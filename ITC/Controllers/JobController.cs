using ITC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace ITC.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        public ActionResult Index()
        {
            ITCContext _dbITC = new ITCContext();
            MP2Context _dbMP2 = new MP2Context();
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();

            ViewBag.BindDDLWorkRequest = QueryRequest.ListJobWorkRequest().Where(w => w.Requestor == emp_no && w.Status <= 3).GroupBy(g => g.WorkRequest).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();
            ViewBag.BindDDLSwOnHand = QuerySoftware.ListSoftwareOnHand().Where(s => s.EmployeeNo == emp_no).OrderBy(o => o.Equipment).ToList();
            ViewBag.BindDDLHwOnHand = QueryHardware.ListHardware().Where(w => w.OWNER == emp_no || w.OWNER.ToUpper() == "ALL").ToList();
            ViewBag.BindDDLAnother = QueryAnother.ListAnotherOnHand(emp_no).ToList();

            return View();
        }

        public string getAgeMachine(DateTime recieveDate)
        {
            double ApproxDaysPerMonth = 30.4375;
            double ApproxDaysPerYear = 365.25;

            int iDays = (DateTime.Now - recieveDate).Days;

            int iYear = (int)(iDays / ApproxDaysPerYear);
            iDays -= (int)(iYear * ApproxDaysPerYear);

            int iMonths = (int)(iDays / ApproxDaysPerMonth);
            iDays -= (int)(iMonths * ApproxDaysPerMonth);

            var fotmatStr = string.Empty;
            if (iYear > 0)
                fotmatStr = iYear + "Year ";
            if (iMonths > 0)
                fotmatStr += iMonths + "Month ";
            if (iDays > 0)
                fotmatStr += iDays + "Day";

            return fotmatStr;
        }

        [HttpPost]
        public JsonResult CreateWorkRequest()
        {
            var wr = string.Empty;
            bool status = false;
            var msg = string.Empty;
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();

            ITCContext _db = new ITCContext();
            List<JobReqHeader> query = _db.JobReqHeader.OrderByDescending(o => o.WorkRequest).Take(1).ToList();

            int countWRLimit = QueryRequest.ListJobWorkRequest().Where(w => w.Requestor == emp_no && w.Status <= 2).Count();
            string year = DateTime.Now.ToString("yyyy") + "-";
            if (countWRLimit < 2)
            {
                status = true;
                msg = "Successful";
                if (query.Count() > 0)
                {
                    switch ((Convert.ToInt32(query[0].WorkRequest.Substring(7)) + 1).ToString().Length)
                    {
                        case 1:
                            wr = "WR" + year + "0000" + (Convert.ToInt32(query[0].WorkRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 2:
                            wr = "WR" + year + "000" + (Convert.ToInt32(query[0].WorkRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 3:
                            wr = "WR" + year + "00" + (Convert.ToInt32(query[0].WorkRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 4:
                            wr = "WR" + year + "0" + (Convert.ToInt32(query[0].WorkRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 5:
                            wr = "WR" + year + (Convert.ToInt32(query[0].WorkRequest.Substring(7)) + 1).ToString();
                            break;
                    }
                }
                else
                {
                    wr = "WR" + year + "00001";
                }

                _db.JobReqHeader.Add(new JobReqHeader
                {
                    WorkRequest = wr,
                    CreateDate = DateTime.Now,
                    Requestor = emp_no
                });

                _db.SaveChanges();
            }
            else
            {
                status = false;
                msg = "Overworked Request";
            }

            return Json(new
            {
                success = status,
                message = msg,
                obj = QueryRequest.ListJobWorkRequest().Where(w => w.Requestor == emp_no && w.Status <= 3).GroupBy(g => g.WorkRequest).OrderByDescending(o => o.Key).Select(s => s.Key).ToList()
            });
        }

        [HttpGet]
        public JsonResult GetJobRequest(string work_request)
        {
            TableJobRequest data = new TableJobRequest()
            {
                data = QueryRequest.ListJobRequest(work_request).Where(w => w.Status == 0 || w.Status == 1).OrderByDescending(o => o.Id).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEquipment(ParameterJobReqBody cc)
        {
            ITCContext _dbITC = new ITCContext();
            string desc = string.Empty;
            List<Symptom> objSymptom = new List<Symptom>();
            if (cc.Equipment == "OtherHW")
            {
                objSymptom = _dbITC.Symptom.Where(w => w.SectionType == "IT").ToList();
            }
            else if (cc.Equipment == "OtherSW")
            {
                objSymptom = _dbITC.Symptom.Where(w => w.SectionType.StartsWith("IS")).ToList();
            }
            else
            {
                desc = QueryAnother.ListAnother().Where(w => w.Equipment == cc.Equipment).ToList()[0].Description;
                switch (cc.Equipment.Substring(0, 2))
                {
                    case "0E":
                        objSymptom = _dbITC.Symptom.Where(w => w.SectionType == "IT").ToList();
                        break;
                    default:
                        objSymptom = _dbITC.Symptom.Where(w => w.SectionType.StartsWith("IS")).ToList();
                        break;
                }
            }

            return Json(new
            {
                val = desc,
                obj = objSymptom
            });
        }

        [HttpPost]
        public JsonResult GetStandardDate(ParameterSymtom cc)
        {
            ITCContext _dbITC = new ITCContext();
            Symptom query = _dbITC.Symptom.Where(w => w.SymptomName == cc.SymptomName).FirstOrDefault();

            return Json(new
            {
                date = String.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(query.StandardDate))
            });
        }

        [HttpPost]
        public JsonResult SetJobReqBody(ParameterJobReqBody cc)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            int line = 0;
            List<JobReqBody> query = _dbITC.JobReqBody.Where(w => w.WorkRequest == cc.WorkRequest).OrderByDescending(o => o.Line).Take(1).ToList();
            line = (query.Count() > 0) ? query[0].Line + 1 : 1;
            var responsible = QueryEquipment.ListEquipment().Where(w => w.Equipment == cc.Equipment).ToList();

            if (cc.Detail == null || cc.RequireDate == "")
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.JobReqBody.Add(new JobReqBody
                    {
                        Equipment = cc.Equipment,
                        Description = (cc.Description == "") ? "N/A" : cc.Description,
                        Symptom = cc.Symptom,
                        Detail = cc.Detail.Replace("\n", Environment.NewLine),
                        RequireDate = Convert.ToDateTime(cc.RequireDate),
                        Status = 0,
                        Line = line,
                        WorkRequest = cc.WorkRequest,
                        Responsible = (responsible.Count > 0) ? responsible[0].Responsible : "",
                        AssignTo = (responsible.Count > 0) ? responsible[0].Responsible : ""
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return Json(new { success = status, message = msg });
        }

        [HttpDelete]
        public JsonResult DeleteJobRequest(int id)
        {
            ITCContext _dbITC = new ITCContext();
            JobReqBody query = _dbITC.JobReqBody.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.JobReqBody.Remove(query);
            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult GetJobReqDetail(int id)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> ListJobWorkRequest = QueryRequest.ListJobWorkRequestInfo().Where(w => w.Id == id).ToList();

            return Json(ListJobWorkRequest, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult SendJobRequest(int id)
        {
            bool status = false;
            string msg = string.Empty;
            string titleEmail = "IT Connects Admin";
            string subject = "";
            string content = "";
            string email_to = "";
            ITCContext _dbITC = new ITCContext();
            sentEmail sm = new sentEmail();
            try
            {
                status = true;
                msg = "Successful";
                JobReqBody queryMisFlow = _dbITC.JobReqBody.Where(w => w.Id == id).FirstOrDefault();
                List<JobRequest> querySymptom = QueryRequest.ListJobRequestJoinSymptom().Where(w => w.Id == id).ToList();
                var data = querySymptom[0];
                switch (data.DecisionType)
                {
                    case "A":
                        subject = "ITC-Work Request Approval (User Manager) : " + data.WorkRequest + " (Need Approve)";
                        content +=
                            "<b>" +
                            "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                            "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + " (" + data.SUBLOCATION3 + ")" + "<br>" +
                            "<span style='color:#007acc;'>Symptom : </span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                            "<span style='color:#007acc;'>Require Date : </span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                            "<span style='color:#007acc;'>Detail : </span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br><br>" +
                            "<span style='color:#007acc;'>Please task one of the following actions :</span> " + "<br>" +
                            "<ul>" +
                            "<li>If you <b>accept</b> equipment, Please click here " +
                            "<a href='http://milws/ITC/Job/ApproveJob?id=" + data.Id + "' style='display: inline-block;font-weight: 400;text-align: center;vertical-align: middle;border: 1px solid transparent;padding: .25rem .5rem;font-size: .875rem;line-height: 1.5;border-radius: .2rem;transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;color: #fff;background-color: #28a745;border-color: #28a745;text-decoration-line:none;'>Accept</a></li><br>" +
                            "<li>If you <b>reject</b> equipment, Please click here " +
                            "<a href='http://milws/ITC/Job/RejectJob?id=" + data.Id + "' style='display: inline-block;font-weight: 400;text-align: center;vertical-align: middle;border: 1px solid transparent;padding: .25rem .5rem;font-size: .875rem;line-height: 1.5;border-radius: .2rem;transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;color: #fff;background-color: #dc3545;border-color: #dc3545;text-decoration-line:none;'>Reject</a></li>" +
                            "</ul></b>";

                        queryMisFlow.Status = 1;
                        _dbITC.SaveChanges();
                        break;
                    case "N":
                        subject = "ITC-Work Request (User Manager) : " + data.WorkRequest + " (Acknowledge)";
                        content +=
                            "<b>" +
                            "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                            "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + " (" + data.SUBLOCATION3 + ")" + "<br>" +
                            "<span style='color:#007acc;'>Symptom : </span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                            "<span style='color:#007acc;'>Require Date : </span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                            "<span style='color:#007acc;'>Detail : </span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "</b><br>";

                        queryMisFlow.Status = 2;
                        _dbITC.SaveChanges();
                        break;
                    case "T":

                        subject = "ITC-Work Request (User Manager) : " + data.WorkRequest + " (Ticket)";
                        content +=
                            "<b>" +
                            "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                            "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + " (" + data.SUBLOCATION3 + ")" + "<br>" +
                            "<span style='color:#007acc;'>Symptom : </span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                            "<span style='color:#007acc;'>Require Date : </span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "</b><br>" +
                            "<span style='color:#007acc;'>Detail : </span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>";

                        var wt = string.Empty;
                        string year = DateTime.Now.ToString("yyyy") + "-";
                        List<WorkOrders> queryWo = _dbITC.WorkOrders.Where(w => w.WoNo.StartsWith("WT")).OrderByDescending(o => o.WoNo).ToList();
                        if (queryWo.Count() > 0)
                        {
                            switch ((Convert.ToInt32(queryWo[0].WoNo.Substring(7)) + 1).ToString().Length)
                            {
                                case 1:
                                    wt = "WT" + year + "0000" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                    break;
                                case 2:
                                    wt = "WT" + year + "000" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                    break;
                                case 3:
                                    wt = "WT" + year + "00" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                    break;
                                case 4:
                                    wt = "WT" + year + "0" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                    break;
                                case 5:
                                    wt = "WT" + year + (Convert.ToInt32(queryWo[0].WoNo.Substring(7)) + 1).ToString();
                                    break;
                            }
                        }
                        else
                        {
                            wt = "WT" + year + "00001";
                        }
                        _dbITC.WorkOrders.Add(new WorkOrders
                        {
                            WoNo = wt,
                            CreateDate = DateTime.Now,
                            Rework = 0,
                            Status = true,
                            JobReqBody_Id = id,
                            Progress = 0
                        });

                        queryMisFlow.Status = 4;

                        switch (data.Equipment.Substring(0, 2))
                        {
                            case "0E":
                                HardwareAsset _HardwareAssets = _dbITC.HardwareAssets.Where(w => w.Equipment.StartsWith(data.Equipment)).FirstOrDefault();
                                queryMisFlow.AssignTo = (_HardwareAssets == null) ? data.Responsible : _HardwareAssets.SupportBy;
                                break;
                            default:
                                Software _Software = _dbITC.Software.Where(w => w.Equipment.StartsWith(data.Equipment)).FirstOrDefault();
                                queryMisFlow.AssignTo = (_Software == null) ? data.Responsible : _Software.SupportBy;
                                break;
                        }

                        _dbITC.SaveChanges();
                        break;
                }
                email_to = QueryRequest.StrEmailApprover(id);
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, email_to, "", "", subject, content, true, "");


                var content2 =
                "<b>" +
                "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + " (" + data.SUBLOCATION3 + ")" + "<br>" +
                "<span style='color:#007acc;'>Symptom : </span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                "<span style='color:#007acc;'>Require Date : </span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "</b><br>" +
                "<span style='color:#007acc;'>Detail : </span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>";
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, "RonnapornK@meyer-mil.com", "", "", subject, content2, true, "");
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        public ActionResult ApproveJob(int id)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            JobReqBody query = _dbITC.JobReqBody.Where(w => w.Id == id).FirstOrDefault();
            if (query.Status == 1)
            {
                query.Status = 2;
                _dbITC.JobApprove.Add(new JobApprove
                {
                    JobReqBody_Id = id,
                    ApproveBy = emp_no,
                    CreateDate = DateTime.Now
                });
                _dbITC.SaveChanges();
            }
            return View();
        }

        public ActionResult RejectJob()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetRejectJob(ParameterRejectJob cc)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                JobReqBody query = _dbITC.JobReqBody.Where(w => w.Id == cc.Id).FirstOrDefault();
                if (query.Status == 1)
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.RejectJobApprover.Add(new RejectJobApprover
                    {
                        Reason = cc.Reason.Replace("\n", Environment.NewLine),
                        JobReqBody_Id = cc.Id,
                        RejectBy = emp_no
                    });
                    query.Status = 10;
                    _dbITC.SaveChanges();
                }
                else
                {
                    status = false;
                    msg = "This process has passed";
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult GetJobStatus()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();

            TableJobPlanning data = new TableJobPlanning()
            {
                data = QueryRequest.ListJobPlanning().Where(w => w.Requestor == emp_no && w.Status != 9).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.Line).ThenByDescending(t2 => t2.Rework).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobStatusInfo(int id, int WorkOrder_Id)
        {
            List<JobRequest> ListJobRequest = new List<JobRequest>();
            if (WorkOrder_Id == 0)
            {
                ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.Id == id).ToList();
            }
            else
            {
                ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == WorkOrder_Id).ToList();
            }

            return Json(ListJobRequest, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFeedJob(int id, int WorkOrder_Id)
        {
            List<JobRequest> ListJobRequest = new List<JobRequest>();
            if (WorkOrder_Id == 0)
            {
                ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.Id == id).ToList();
            }
            else
            {
                ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == WorkOrder_Id).ToList();
            }

            return Json(ListJobRequest, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult SetAcceptJob(int id)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                JobReqBody queryJobReqBody = _dbITC.JobReqBody.Where(w => w.Id == id).FirstOrDefault();
                queryJobReqBody.Status = 9;
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }


        [HttpPost]
        public JsonResult SetNotAcceptJob(ParameterJobRework cc)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                JobReqBody queryJobReqBody = _dbITC.JobReqBody.Where(w => w.Id == cc.Id).FirstOrDefault();
                queryJobReqBody.Status = 8;

                _dbITC.JobUnAccept.Add(new JobUnAccept
                {
                    Reason = cc.Reason.Replace("\n", Environment.NewLine),
                    WorkOrder_Id = cc.WorkOrder_Id
                });

                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult GetReasonJobReject(int id)
        {
            return Json(QueryRequest.ListJobReject().Where(w => w.Id == id).ToList(), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetReasonJobRework(int WorkOrder_Id)
        {
            return Json(QueryRequest.ListJobRework().Where(w => w.WorkOrder_Id == WorkOrder_Id).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChartStatus()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            List<StatusChart> objStatus = new List<StatusChart>();
            StatusChart itemStatus;
            List<JobRequest> _ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.Requestor == emp_no && w.Status > 0 && Convert.ToDateTime(w.CreateDate).ToString("yyyy") == DateTime.Now.ToString("yyyy"))
                                .Select(s => new JobRequest
                                {
                                    StatusStr = (s.Status == 9 && s.StatusWorkOrder == true) ? "COMPLETED" : (((s.Status == 6 || s.Status == 7 || s.Status == 9) && s.StatusWorkOrder == false) || s.Status == 12 || s.Status == 13) ? "REWORKED" : (s.Status == 10 || s.Status == 11) ? "REJECTED" : (s.Status == 1) ? "PENDING" : "PROCEED"
                                }).ToList();

            _ListJobRequest = _ListJobRequest.GroupBy(g => new
            {
                g.StatusStr,
            }
                                , (key, group) => new JobRequest
                                {
                                    StatusStr = key.StatusStr,
                                    CountStatus = group.Count()
                                }).ToList();

            for (int i = 0; i < _ListJobRequest.Count(); i++)
            {
                itemStatus = new StatusChart();
                itemStatus.Status = _ListJobRequest[i].StatusStr;
                itemStatus.Color = (_ListJobRequest[i].StatusStr == "COMPLETED") ? "#28a745" : (_ListJobRequest[i].StatusStr == "REWORKED") ? "#6c757d" : (_ListJobRequest[i].StatusStr == "REJECTED") ? "#dc3545" : (_ListJobRequest[i].StatusStr == "PENDING") ? "#a333c8" : "#ffc107";
                itemStatus.CountStatus = _ListJobRequest[i].CountStatus;
                objStatus.Add(itemStatus);
            }

            return Json(new { obj = objStatus.ToList() });
        }

        public PartialViewResult FeedJob(int id)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ViewBag.BindData = QueryRequest.ListJobComments(id).ToList();
            ViewBag.BindEmpNo = emp_no;
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetFeed(ParameterSetFeed cc)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            try
            {
                status = true;
                switch (cc.TypeFeed)
                {
                    case 1:
                        _dbITC.JobComment.Add(new JobComment
                        {
                            Title = cc.TitleComment,
                            Comment = cc.Comment.Replace("\n", Environment.NewLine),
                            JobReqBody_Id = cc.JobReqBody_Id,
                            CreateBy = emp_no,
                            CreateDate = DateTime.Now
                        });
                        break;
                    case 2:
                        _dbITC.JobReply.Add(new JobReply
                        {
                            Reply = cc.Comment.Replace("\n", Environment.NewLine),
                            JobComment_Id = cc.TitleReply,
                            JobReqBody_Id = cc.JobReqBody_Id,
                            CreateBy = emp_no,
                            CreateDate = DateTime.Now
                        });
                        break;
                }
                _dbITC.SaveChanges();

                string titleEmail = "IT Connects Admin";
                string subject = "";
                string content = "";
                string contentReply = "";
                var head = "";
                string email_to = "";
                sentEmail sm = new sentEmail();

                subject = "ITC-FEED JOB : " + cc.WorkRequest + " (" + cc.Equipment + ")";

                head = "<b>" + cc.WorkRequest + " : " + cc.Equipment + "</b>";

                for (int i = 0; i < QueryRequest.ListJobComments(cc.Id).ToList().Count; i++)
                {
                    var item = QueryRequest.ListJobComments(cc.Id).ToList()[i];
                    contentReply = "";
                    for (int j = 0; j < item.ObjJobReply.Count; j++)
                    {
                        var itemReply = item.ObjJobReply[j];
                        contentReply += "<div class='media mt-2' style='flex: 1;margin-top: .3rem!important;'>" +
                                                   "<a class='mr-2' style='margin-right: .5rem!important;'></a>" +
                                                        "<div class='media-body' style='flex: 1;margin-left:15px;'>" +
                                                            "Reply => " + itemReply.Reply.Replace(Environment.NewLine, "<br />") + "<br/>" +
                                                            "<label style='font-size:xx-small;'>BY " + itemReply.EmployeeName + " : " + itemReply.CreateDate + "</label>" +
                                                        "</div>" +
                                         "</div>";
                    }
                    content += "<div class='media mb-2' style='display: flex; align-items: flex-start;'>" +
                                   "<div class='mr-2' style='margin-right: .5rem!important;font-weight:bold;'>##</div>" +
                                   "<div class='media-body' style='flex: 1;'>" +
                                       "<h5 class='mt-0' style='margin-top: 0!important;font-size: 1.00rem;margin-bottom: .5rem;font-weight: bold;line-height: 1.2;'>" + item.Title + "</h5>" +
                                       "Comment => " + item.Comment.Replace(Environment.NewLine, "<br />") + "<br/>" +
                                       "<label style='font-size:xx-small;'>BY " + item.EmployeeName + " : " + item.CreateDate + "</label>" +
                                       contentReply +
                                     "</div>" +
                               "</div><hr><br>";
                }
                content = head + "<br><br>" + content;
                email_to = QueryRequest.StrEmailResponsible(cc.JobReqBody_Id) + ";" + QueryRequest.StrEmailPlanner(cc.JobReqBody_Id);
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, email_to, "", "", subject, content, true, "");
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult GetTitleFeed(int id)
        {
            ITCContext _dbITC = new ITCContext();
            return Json(_dbITC.JobComment.Where(w => w.JobReqBody_Id == id).ToList().OrderBy(o => o.Id), JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteFeed(int id, string type)
        {
            ITCContext _dbITC = new ITCContext();
            var JobReqBody_Id = 0;
            switch (type)
            {
                case "comment":
                    JobComment qc = _dbITC.JobComment.FirstOrDefault(s => s.Id == id);
                    _dbITC.JobComment.Remove(qc);
                    JobReqBody_Id = qc.JobReqBody_Id;
                    break;
                case "reply":
                    JobReply qr = _dbITC.JobReply.FirstOrDefault(s => s.Id == id);
                    _dbITC.JobReply.Remove(qr);
                    JobReqBody_Id = qr.JobReqBody_Id;
                    break;
            }
            _dbITC.SaveChanges();
            return Json(new { id = JobReqBody_Id, success = true, message = "Delete successful" });
        }
    }
}