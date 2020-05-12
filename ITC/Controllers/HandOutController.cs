using ITC.Models;
using ITC.MyAppHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace ITC.Controllers
{
    [Authorize]
    public class HandOutController : Controller
    {
        public ActionResult Index()
        {
            ITCContext _dbITC = new ITCContext();
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            MisFlow query = _dbITC.MisFlow.Where(w => w.EmployeeNo == emp_no && w.JobType == "Planner").FirstOrDefault();
            string division = query.Division;
            ViewBag.BindDDLSymptom = _dbITC.Symptom.Where(w => w.SectionType.StartsWith(division)).ToList();
            ViewBag.BindDDLAssignTo = QueryMisFlow.ListMisFlow().Where(w => w.Division == division && w.JobType == "Staff").ToList();
            ViewBag.BindDDLWorkRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && (w.Status == 4 || w.Status == 5)).GroupBy(g => g.WorkRequest).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();
            ViewBag.BindDDlWorkOrder = QueryRequest.ListJobPlanning().Where(w => w.SectionType.StartsWith(query.Division) && ((w.Status == 4 && w.StatusWorkOrder == false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13)).GroupBy(g => g.WoNo).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();
            ViewBag.BindDDLAssignTo = QueryMisFlow.ListMisFlow().Where(w => w.Division.StartsWith(query.Division) && w.JobType == "Staff").ToList();

            return View();
        }

        [HttpGet]
        public JsonResult GetJobResponsible()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            MisFlow query = _dbITC.MisFlow.Where(w => w.EmployeeNo == emp_no && w.JobType == "Planner").FirstOrDefault();

            TableJobResponsible data = new TableJobResponsible()
            {
                data = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && (w.Status == 2 || w.Status == 3 || w.Status == 12)).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.Line).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAssignJob(int id)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = QueryRequest.ListJobRequestJoinSymptom().Where(w => w.Id == id).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetJobReqBody(ParameterJobReqBody cc)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                if (cc.Symptom == "0" || cc.Responsible == "0" || cc.Suggestion == null)
                {
                    status = false;
                    msg = "One or more fields have _blank";
                }
                else
                {
                    status = true;
                    msg = "Successful";
                    JobReqBody query = _dbITC.JobReqBody.Where(w => w.Id == cc.Id).FirstOrDefault();
                    List<SolutionSuggestion> querySolutionSuggestion = _dbITC.SolutionSuggestion.Where(w => w.JobReqBody_Id == cc.Id).ToList();

                    if (querySolutionSuggestion.Count() > 0)
                    {

                        querySolutionSuggestion[0].Solution = cc.Suggestion.Replace("\n", Environment.NewLine);
                    }
                    else
                    {
                        _dbITC.SolutionSuggestion.Add(new SolutionSuggestion
                        {
                            Solution = cc.Suggestion.Replace("\n", Environment.NewLine),
                            JobReqBody_Id = cc.Id
                        });
                    }

                    query.Symptom = cc.Symptom;
                    query.AssignTo = cc.AssignTo;
                    _dbITC.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpPut]
        public JsonResult SendAssignJob(int id)
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
                List<JobReqBody> queryMisFlow = _dbITC.JobReqBody.Where(w => w.Id == id).ToList();
                List<JobRequest> querySymptom = QueryRequest.ListJobRequestJoinSymptom().Where(w => w.Id == id).ToList();
                var data = querySymptom[0];

                switch (data.DecisionType)
                {
                    case "A":
                        if (queryMisFlow[0].Status == 12)
                        {
                            queryMisFlow[0].Status = 13;
                            _dbITC.SaveChanges();
                        }
                        else
                        {
                            queryMisFlow[0].Status = 3;
                            _dbITC.SaveChanges();
                        }

                        subject = "ITC-Work Request Approval : " + data.WorkRequest;
                        content +=
                            "<b>" +
                            "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                            "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + "<br>" +
                            "<span style='color:#007acc;'>Symptom : </span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                            "<span style='color:#007acc;'>Require Date : </span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                            "<span style='color:#007acc;'>Responsible : </span> " + data.ResponsibleName + " (" + data.Responsible + ")" + "<br>" +
                            "<span style='color:#007acc;'>Assign To : </span> " + data.AssignToName + " (" + data.AssignTo + ")" + "<br>" +
                            "<span style='color:#007acc;'>Detail : </span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                            "<span style='color:#007acc;'>Solution Suggestion : </span> " + data.Suggestion.Replace(Environment.NewLine, "<br>") + "<br><br>" +
                            "<span style='color:#007acc;'>Approve by : </span> " + data.ApproveByName + " (User Manager)" + "<br>" +
                            "<span style='color:#007acc;'>Approve Date : </span> " + data.ApproveDate + "<br><br>" +
                            "<span style='color:#007acc;'>Please task one of the following actions :</span> " +
                            "<ul>" +
                            "<li>If you <b>accept</b> equipment, Please click here " +
                            "<a href='http://milws/ITC/HandOut/ApproveJob?id=" + data.Id + "' style='display: inline-block;font-weight: 400;text-align: center;vertical-align: middle;border: 1px solid transparent;padding: .25rem .5rem;font-size: .875rem;line-height: 1.5;border-radius: .2rem;transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;color: #fff;background-color: #28a745;border-color: #28a745;text-decoration-line:none;'>Accept</a></li><br>" +
                            "<li>If you <b>reject</b> equipment, Please click here " +
                            "<a href='http://milws/ITC/HandOut/RejectJob?id=" + data.Id + "' style='display: inline-block;font-weight: 400;text-align: center;vertical-align: middle;border: 1px solid transparent;padding: .25rem .5rem;font-size: .875rem;line-height: 1.5;border-radius: .2rem;transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;color: #fff;background-color: #dc3545;border-color: #dc3545;text-decoration-line:none;'>Reject</a></li>" +
                            "</ul></b>";
                        break;
                    case "N":
                        if (queryMisFlow[0].Status == 12)
                        {
                            List<WorkOrders> queryWo = _dbITC.WorkOrders.Where(w => w.JobReqBody_Id == id).OrderByDescending(o => o.Rework).ToList();
                            _dbITC.WorkOrders.Add(new WorkOrders
                            {
                                WoNo = queryWo[0].WoNo,
                                CreateDate = DateTime.Now,
                                Rework = queryWo[0].Rework + 1,
                                Status = true,
                                JobReqBody_Id = id,
                                Progress = 0
                            });
                            queryWo[0].Status = false;
                            queryMisFlow[0].Status = 4;
                            _dbITC.SaveChanges();
                        }
                        else
                        {
                            var wo = string.Empty;
                            string year = DateTime.Now.ToString("yyyy") + "-";
                            List<WorkOrders> queryWo = _dbITC.WorkOrders.Where(w => w.WoNo.StartsWith("WO")).OrderByDescending(o => o.WoNo).ToList();
                            if (queryWo.Count() > 0)
                            {
                                switch ((Convert.ToInt32(queryWo[0].WoNo.Substring(7)) + 1).ToString().Length)
                                {
                                    case 1:
                                        wo = "WO" + year + "0000" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                        break;
                                    case 2:
                                        wo = "WO" + year + "000" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                        break;
                                    case 3:
                                        wo = "WO" + year + "00" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                        break;
                                    case 4:
                                        wo = "WO" + year + "0" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                                        break;
                                    case 5:
                                        wo = "WO" + year + (Convert.ToInt32(queryWo[0].WoNo.Substring(7)) + 1).ToString();
                                        break;
                                }
                            }
                            else
                            {
                                wo = "WO" + year + "00001";
                            }
                            _dbITC.WorkOrders.Add(new WorkOrders
                            {
                                WoNo = wo,
                                CreateDate = DateTime.Now,
                                Rework = 0,
                                Status = true,
                                JobReqBody_Id = id,
                                Progress = 0
                            });

                            queryMisFlow[0].Status = 4;
                            _dbITC.SaveChanges();
                        }

                        subject = "ITC-Work Request : " + data.WorkRequest;
                        content +=
                            "<b>" +
                            "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                            "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + "<br>" +
                            "<span style='color:#007acc;'>Symptom :</span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                            "<span style='color:#007acc;'>Require Date :</span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                            "<span style='color:#007acc;'>Responsible :</span> " + data.ResponsibleName + "<br>" +
                            "<span style='color:#007acc;'>Assign To :</span> " + data.AssignToName + "<br>" +
                            "<span style='color:#007acc;'>Detail :</span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                            "<span style='color:#007acc;'>Solution Suggestion :</span> " + data.Suggestion.Replace(Environment.NewLine, "<br>") + "<br>";
                        break;
                    case "T":
                        if (queryMisFlow[0].Status == 12)
                        {
                            List<WorkOrders> queryWo = _dbITC.WorkOrders.Where(w => w.JobReqBody_Id == id).OrderByDescending(o => o.Rework).ToList();
                            _dbITC.WorkOrders.Add(new WorkOrders
                            {
                                WoNo = queryWo[0].WoNo,
                                CreateDate = DateTime.Now,
                                Rework = queryWo[0].Rework + 1,
                                Status = true,
                                JobReqBody_Id = id,
                                Progress = 0
                            });
                            queryWo[0].Status = false;
                            queryMisFlow[0].Status = 4;
                            _dbITC.SaveChanges();
                        }
                        else
                        {
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

                            queryMisFlow[0].Status = 4;
                            _dbITC.SaveChanges();
                        }

                        subject = "ITC-Work Request : " + data.WorkRequest;
                        content +=
                            "<b>" +
                            "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                            "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + "<br>" +
                            "<span style='color:#007acc;'>Symptom :</span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                            "<span style='color:#007acc;'>Require Date :</span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                            "<span style='color:#007acc;'>Responsible :</span> " + data.ResponsibleName + "<br>" +
                            "<span style='color:#007acc;'>Assign To :</span> " + data.AssignToName + "<br>" +
                            "<span style='color:#007acc;'>Detail :</span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                           "<span style='color:#007acc;'>Solution Suggestion :</span> " + data.Suggestion.Replace(Environment.NewLine, "<br>") + "<br>";
                        break;
                }
                email_to = QueryRequest.StrEmailManager();
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, email_to, "", "", subject, content, true, "");

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
            var wo = string.Empty;
            string year = DateTime.Now.ToString("yyyy") + "-";
            ITCContext _dbITC = new ITCContext();
            JobReqBody query = _dbITC.JobReqBody.Where(w => w.Id == id).FirstOrDefault();
            if (query.Status == 3)
            {
                List<WorkOrders> queryWo = _dbITC.WorkOrders.Where(w => w.WoNo.StartsWith("WO")).OrderByDescending(o => o.WoNo).ToList();
                if (queryWo.Count() > 0)
                {
                    switch ((Convert.ToInt32(queryWo[0].WoNo.Substring(7)) + 1).ToString().Length)
                    {
                        case 1:
                            wo = "WO" + year + "0000" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 2:
                            wo = "WO" + year + "000" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 3:
                            wo = "WO" + year + "00" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 4:
                            wo = "WO" + year + "0" + (Convert.ToInt32(queryWo[0].WoNo.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 5:
                            wo = "WO" + year + (Convert.ToInt32(queryWo[0].WoNo.Substring(7)) + 1).ToString();
                            break;
                    }
                }
                else
                {
                    wo = "WO" + year + "00001";
                }

                _dbITC.WorkOrders.Add(new WorkOrders
                {
                    WoNo = wo,
                    CreateDate = DateTime.Now,
                    Rework = 0,
                    Status = true,
                    JobReqBody_Id = id,
                    Progress = 0
                });

                query.Status = 4;
                _dbITC.SaveChanges();

                sentEmail sm = new sentEmail();
                string titleEmail = "IT Connects Admin";
                string subject = "";
                string content = "";
                string email_to = "";
                List<JobRequest> querySymptom = QueryRequest.ListJobRequestJoinSymptom().Where(w => w.Id == id).ToList();
                var data = querySymptom[0];
                subject = "ITC-Work Request : " + data.WorkRequest + " (Approved)";
                content +=
                    "<b>" +
                    "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                    "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + "<br>" +
                    "<span style='color:#007acc;'>Symptom :</span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                    "<span style='color:#007acc;'>Require Date :</span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                    "<span style='color:#007acc;'>Responsible :</span> " + data.ResponsibleName + "<br>" +
                    "<span style='color:#007acc;'>Assign To :</span> " + data.AssignToName + "<br>" +
                    "<span style='color:#007acc;'>Detail :</span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                    "<span style='color:#007acc;'>Solution Suggestion :</span> " + data.Suggestion.Replace(Environment.NewLine, "<br>") + "<br>";

                email_to = QueryRequest.StrEmailPlanner(data.AssignTo);
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, email_to, "", "", subject, content, true, "");
            }
            else if (query.Status == 13)
            {
                WorkOrders queryWo = _dbITC.WorkOrders.Where(w => w.JobReqBody_Id == id).FirstOrDefault();
                _dbITC.WorkOrders.Add(new WorkOrders
                {
                    WoNo = queryWo.WoNo,
                    CreateDate = DateTime.Now,
                    Rework = queryWo.Rework + 1,
                    Status = true,
                    JobReqBody_Id = id,
                    Progress = 0
                });
                queryWo.Status = false;
                query.Status = 4;

                _dbITC.SaveChanges();
            }

            return View();
        }

        public ActionResult RejectJob()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJobPlanning(ParameterJobReqBody cc)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            MisFlow query = _dbITC.MisFlow.Where(w => w.EmployeeNo == emp_no && w.JobType == "Planner").FirstOrDefault();
            List<JobRequest> ListQueryRequest = new List<JobRequest>();
            if (cc.WorkRequest == "0" && cc.FieldsDate == "0")
            {
                ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && ((w.Status == 4 || w.Status == 5) && w.StatusWorkOrder == true)).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.Line).ThenByDescending(t => t.Rework).ToList().ToList();
            }
            else
            {
                if (cc.FieldsDate == "0")
                {
                    ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && ((w.Status == 4 || w.Status == 5) && w.StatusWorkOrder == true) && w.WorkRequest.Contains(cc.WorkRequest)).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.Line).ThenByDescending(t => t.Rework).ToList().ToList();
                }
                else
                {
                    if (cc.FieldsDate == "CD" && cc.FromDate != null && cc.ToDate != null)
                    {
                        ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && ((w.Status == 4 || w.Status == 5) && w.StatusWorkOrder == true) && w.WorkRequest.Contains(cc.WorkRequest) && (Convert.ToDateTime(w.CreateDate) >= Convert.ToDateTime(cc.FromDate) && Convert.ToDateTime(w.CreateDate) <= Convert.ToDateTime(cc.ToDate))).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.Line).ThenByDescending(t => t.Rework).ToList();
                    }
                    else if (cc.FieldsDate == "RD" && cc.FromDate != null && cc.ToDate != null)
                    {
                        ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && ((w.Status == 4 || w.Status == 5) && w.StatusWorkOrder == true) && w.WorkRequest.Contains(cc.WorkRequest) && (Convert.ToDateTime(w.RequireDate) >= Convert.ToDateTime(cc.FromDate) && Convert.ToDateTime(w.RequireDate) <= Convert.ToDateTime(cc.ToDate))).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.Line).ThenByDescending(t => t.Rework).ToList();
                    }
                    else
                    {
                        ListQueryRequest = new List<JobRequest>();
                    }
                }
            }

            TableJobPlanning data = new TableJobPlanning()
            {
                data = ListQueryRequest
            };
            return Json(data, JsonRequestBehavior.AllowGet);
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
                if (query.Status == 3)
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.RejectJobApprover.Add(new RejectJobApprover
                    {
                        Reason = cc.Reason.Replace("\n", Environment.NewLine),
                        JobReqBody_Id = cc.Id,
                        RejectBy = emp_no
                    });
                    query.Status = 11;
                    _dbITC.SaveChanges();

                    sentEmail sm = new sentEmail();
                    string titleEmail = "IT Connects Admin";
                    string subject = "";
                    string content = "";
                    string email_to = "";

                    List<JobRequest> querySymptom = QueryRequest.ListJobReqReject().Where(w => w.Id == cc.Id).ToList();
                    var data = querySymptom[0];
                    subject = "ITC-Work Request : " + data.WorkRequest + " (Rejected)";
                    content +=
                        "<b>" +
                        "<span style='color:#007acc;'>Request by : </span> " + data.Requestor + " " + data.RequestorName + "<br>" +
                        "<span style='color:#007acc;'>Equipment : </span> " + data.Equipment + " " + data.Description + "<br>" +
                        "<span style='color:#007acc;'>Symptom :</span> " + data.Symptom + " (" + data.SymptomName_Th + ")" + "<br>" +
                        "<span style='color:#007acc;'>Require Date :</span> " + String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(data.RequireDate)) + "<br>" +
                        "<span style='color:#007acc;'>Responsible :</span> " + data.ResponsibleName + "<br>" +
                        "<span style='color:#007acc;'>Assign To :</span> " + data.AssignToName + "<br>" +
                        "<span style='color:#007acc;'>Detail :</span> " + data.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                        "<span style='color:#007acc;'>Solution Suggestion :</span> " + data.Suggestion.Replace(Environment.NewLine, "<br>") + "<br>" +
                        "<span style='color:#007acc;'>Reason :</span> " + data.Reason.Replace(Environment.NewLine, "<br>") + "<br>";

                    email_to = QueryRequest.StrEmailPlanner(data.AssignTo);
                    sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, email_to, "", "", subject, content, true, "");
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

        public ActionResult ExportJob(ParameterJobReqBody cc)
        {
            Excel excel = new Excel();
            string[] positionDatetime = new string[] { "J", "K" };
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            MisFlow query = _dbITC.MisFlow.Where(w => w.EmployeeNo == emp_no && w.JobType == "Planner").FirstOrDefault();
            List<JobRequest> ListQueryRequest = new List<JobRequest>();
            if (cc.WorkRequest == "0" && cc.FieldsDate == "0")
            {
                ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && (w.Status == 4 || w.Status == 5 && w.StatusWorkOrder == true)).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.WoNo).ToList();
            }
            else
            {
                if (cc.FieldsDate == "0")
                {
                    ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && (w.Status == 4 || w.Status == 5 && w.StatusWorkOrder == true) && w.WorkRequest.Contains(cc.WorkRequest)).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.WoNo).ToList();
                }
                else
                {
                    if (cc.FieldsDate == "CD" && cc.FromDate != null && cc.ToDate != null)
                    {
                        ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && (w.Status == 4 || w.Status == 5 && w.StatusWorkOrder == true) && w.WorkRequest.Contains(cc.WorkRequest) && (Convert.ToDateTime(w.CreateDate) >= Convert.ToDateTime(cc.FromDate) && Convert.ToDateTime(w.CreateDate) <= Convert.ToDateTime(cc.ToDate))).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.WoNo).ToList();
                    }
                    else if (cc.FieldsDate == "RD" && cc.FromDate != null && cc.ToDate != null)
                    {
                        ListQueryRequest = QueryRequest.ListAssignResponsible().Where(w => w.SectionType.StartsWith(query.Division) && (w.Status == 4 || w.Status == 5 && w.StatusWorkOrder == true) && w.WorkRequest.Contains(cc.WorkRequest) && (Convert.ToDateTime(w.RequireDate) >= Convert.ToDateTime(cc.FromDate) && Convert.ToDateTime(w.RequireDate) <= Convert.ToDateTime(cc.ToDate))).OrderByDescending(o => o.WorkRequest).ThenByDescending(t => t.WoNo).ToList();
                    }
                    else
                    {
                        ListQueryRequest = new List<JobRequest>();
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Work Request", typeof(string));
            dt.Columns.Add("Line", typeof(string));
            dt.Columns.Add("Work Order", typeof(string));
            dt.Columns.Add("Rework", typeof(string));
            dt.Columns.Add("Equipment", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Symptom", typeof(string));
            dt.Columns.Add("Priority", typeof(string));
            dt.Columns.Add("Detail", typeof(string));
            dt.Columns.Add("Create Date", typeof(DateTime));
            dt.Columns.Add("Require Date", typeof(DateTime));
            dt.Columns.Add("Requestor", typeof(string));
            dt.Columns.Add("Requestor Name", typeof(string));
            dt.Columns.Add("Responsible", typeof(string));
            dt.Columns.Add("Responsible Name", typeof(string));
            dt.Columns.Add("Assign To", typeof(string));
            dt.Columns.Add("Assign To Name", typeof(string));
            dt.Columns.Add("Section Type", typeof(string));
            dt.Columns.Add("Suggestion", typeof(string));

            for (int i = 0; i < ListQueryRequest.Count(); i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = ListQueryRequest[i].WorkRequest;
                dr[1] = ListQueryRequest[i].Line;
                dr[2] = ListQueryRequest[i].WoNo;
                dr[3] = ListQueryRequest[i].Rework;
                dr[4] = ListQueryRequest[i].Equipment;
                dr[5] = ListQueryRequest[i].Description;
                dr[6] = ListQueryRequest[i].Symptom;
                dr[7] = ListQueryRequest[i].Priority;
                dr[8] = ListQueryRequest[i].Detail;
                dr[9] = ListQueryRequest[i].CreateDate;
                dr[10] = ListQueryRequest[i].RequireDate;
                dr[11] = ListQueryRequest[i].Requestor;
                dr[12] = ListQueryRequest[i].RequestorName;
                dr[13] = ListQueryRequest[i].Responsible;
                dr[14] = ListQueryRequest[i].ResponsibleName;
                dr[15] = ListQueryRequest[i].AssignTo;
                dr[16] = ListQueryRequest[i].AssignToName;
                dr[17] = ListQueryRequest[i].SectionType;
                dr[18] = ListQueryRequest[i].Suggestion;

                dt.Rows.Add(dr);
            }
            excel.GenerateExcel(dt, "sheet1", "JOB_WEEKLY_PLANNING", "JOB_WEEKLY_PLANNING", "", positionDatetime);

            return View();
        }

        [HttpGet]
        public JsonResult GetTrackingJob(ParameterDailyReport cc)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            MisFlow query = _dbITC.MisFlow.Where(w => w.EmployeeNo == emp_no && w.JobType == "Planner").FirstOrDefault();
            List<DailyReport> ListDailyReport = QueryRequest.ListJobDaily().Where(w => w.SectionType.StartsWith(query.Division) && ((w.Status == 4 && w.StatusWorkOrder == false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13))
                                               .GroupBy(g => new
                                               {
                                                   g.WorkOrder_Id,
                                                   g.WoNo,
                                                   g.Rework,
                                                   g.Progress,
                                                   g.Priority,
                                                   g.AssignToName,
                                                   g.AssignTo,
                                                   g.Status,
                                                   g.StatusWorkOrder
                                               }, (key, group) => new DailyReport
                                               {
                                                   WorkOrder_Id = key.WorkOrder_Id,
                                                   WoNo = key.WoNo,
                                                   Rework = key.Rework,
                                                   Progress = key.Progress,
                                                   Priority = key.Priority,
                                                   AssignToName = key.AssignToName,
                                                   AssignTo = key.AssignTo,
                                                   Status = key.Status,
                                                   StatusWorkOrder = key.StatusWorkOrder
                                               }).OrderByDescending(o => o.WoNo).ThenByDescending(t => t.Rework).ToList();

            if (cc.WorkOrder == "0" && cc.AssignTo == "0")
            {
                ListDailyReport = ListDailyReport.ToList();
            }
            else if (cc.WorkOrder == "0" && cc.AssignTo != "0")
            {
                ListDailyReport = ListDailyReport.Where(w => w.AssignTo == cc.AssignTo).ToList();
            }
            else if (cc.WorkOrder != "0" && cc.AssignTo == "0")
            {
                ListDailyReport = ListDailyReport.Where(w => w.WoNo == cc.WorkOrder).ToList();
            }
            else if (cc.WorkOrder != "0" && cc.AssignTo != "0")
            {
                ListDailyReport = ListDailyReport.Where(w => w.WoNo == cc.WorkOrder && w.AssignTo == cc.AssignTo).ToList();
            }


            TableDailyReport data = new TableDailyReport()
            {
                data = ListDailyReport
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTrackingJobInfo(int id)
        {
            List<JobRequest> ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == id).ToList();
            return Json(ListJobRequest, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult JobDaily(string emp_no)
        {
            ViewBag.BindEmployeeNo = emp_no;
            return PartialView();
        }

        public PartialViewResult SchedulePlan(string emp_no)
        {
            ViewBag.BindEmployeeNo = emp_no;
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetInfoDailyReport(ParameterJobDaily cc)
        {
            List<DailyReport> _ListQueryRequest = QueryRequest.ListDailyReport().Where(w => w.WorkOrder_Id == cc.id).ToList();
            TimeSpan duration = new TimeSpan(0, 0, 0, 0);
            for (int i = 0; i < _ListQueryRequest.Count(); i++)
            {
                var item = _ListQueryRequest[i];
                duration = DateTime.Parse(item.TimeStop).Subtract(DateTime.Parse(item.TimeStart)) + duration;
            }
            var total = "";

            if (duration.Hours > 0)
            {
                if (duration.Minutes > 0)
                {
                    total = ((duration.Days * 24) + duration.Hours).ToString() + " Hrs " + duration.Minutes + " Min";
                }
                else
                {
                    total = duration.Hours.ToString() + " Hrs";
                }
            }
            else
            {
                total = duration.Minutes.ToString() + " Min";
            }

            return Json(new { obj = _ListQueryRequest, working = total });
        }

        [HttpGet]
        public JsonResult GetCalendarDaily(string emp_no)
        {
            return Json(QueryRequest.ListCalendarDaily(emp_no), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchedulePlan(string emp_no)
        {
            return Json(QueryRequest.ListSchedulePlan(emp_no), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetRejectWorkOrder(ParameterTrackingJob cc)
        {
            bool status = false;
            string msg = string.Empty;

            ITCContext _dbITC = new ITCContext();
            MILAuthContext _dbMILAuth = new MILAuthContext();
            try
            {
                status = true;
                msg = "Successful";
                WorkOrders _ListWorkOrders = _dbITC.WorkOrders.Where(w => w.Id == cc.Id).FirstOrDefault();
                JobReqBody _ListJobReqBody = _dbITC.JobReqBody.Where(w => w.Id == _ListWorkOrders.JobReqBody_Id).FirstOrDefault();
                JobDaily _ListJobDaily = _dbITC.JobDaily.Where(w => w.WorkOrder_Id == _ListWorkOrders.Id && w.Status == 3).FirstOrDefault();

                _ListWorkOrders.Status = true;
                _ListWorkOrders.Progress = 75;
                _ListJobReqBody.Status = 5;
                _dbITC.JobDaily.Remove(_ListJobDaily);
                _dbITC.SaveChanges();

                string titleEmail = "IT Connects Admin";
                string subject = "";
                string content = "";
                sentEmail sm = new sentEmail();

                subject = "ITC-Reject Work Order : " + _ListWorkOrders.WoNo + "-" + _ListWorkOrders.Rework;
                content += "<b>" + cc.Reason.Replace(Environment.NewLine, "<br>") + "</b>";

                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, QueryRequest.StrEmailPlannerAndStaff(_ListJobReqBody.AssignTo), "", "", subject, content, true, "");
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpPost]
        public JsonResult SetCloseJob(int id)
        {
            bool status = false;
            string msg = string.Empty;

            ITCContext _dbITC = new ITCContext();
            MILAuthContext _dbMILAuth = new MILAuthContext();
            try
            {
                status = true;
                msg = "Successful";

                JobRequest _ListJobPlanning = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == id).FirstOrDefault();
                JobReqBody _ListJobReqBody = _dbITC.JobReqBody.Where(w => w.Id == _ListJobPlanning.Id).FirstOrDefault();
                //JobReqHeader _ListJobReqHeader = _dbITC.JobReqHeader.Where(w => w.WorkRequest == _ListJobReqBody.WorkRequest).FirstOrDefault();
                AccountJoinEmployee _ListAccounts = QueryAccount.ListAccountMeyer().Where(w => w.EmployeeNo == _ListJobPlanning.Requestor).FirstOrDefault();
                _ListJobReqBody.Status = 7;
                _dbITC.SaveChanges();

                string titleEmail = "IT Connects Admin";
                string subject = "";
                string content = "";
                string content2 = "";
                sentEmail sm = new sentEmail();

                subject = "ITC-Complete Work Order : " + _ListJobPlanning.WoNo + "-" + _ListJobPlanning.Rework;
                content += "<b>" +
                    "<span style='color:#007acc;'>Work Request : </span> " + _ListJobPlanning.WorkRequest + "<br>" +
                    "<span style='color:#007acc;'>Equipment : </span> " + _ListJobPlanning.Description + " (" + _ListJobPlanning.Equipment + ")" + "<br>" +
                    "<span style='color:#007acc;'>Symptom : </span> " + _ListJobPlanning.Symptom + " (" + _ListJobPlanning.SymptomName_Th + ")" + "<br>" +
                    "<span style='color:#007acc;'>Detail : </span> " + _ListJobPlanning.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                    "<span style='color:#007acc;'>Require Date : </span> " + _ListJobPlanning.RequireDate + "<br>" +
                    "<span style='color:#007acc;'>Start Date : </span> " + _ListJobPlanning.PlanStartDate + "<br>" +
                    "<span style='color:#007acc;'>Finish Date : </span> " + _ListJobPlanning.PlanFinishDate + "<br>" +
                    "<span style='color:#007acc;'>Support By : </span> " + _ListJobPlanning.AssignToName + "<br><br>" +
                    "<span style='color:#007acc;'>This link below for accept job :</span> " + "<br>" +
                    "<a href='http://milws/ITC/Job/Index'>Click here!</a></b>";
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, _ListAccounts.Email, "", "", subject, content, true, "");

                content2 += "<b>" +
                    "<span style='color:#007acc;'>Work Request : </span> " + _ListJobPlanning.WorkRequest + "<br>" +
                    "<span style='color:#007acc;'>Equipment : </span> " + _ListJobPlanning.Description + " (" + _ListJobPlanning.Equipment + ")" + "<br>" +
                    "<span style='color:#007acc;'>Symptom : </span> " + _ListJobPlanning.Symptom + " (" + _ListJobPlanning.SymptomName_Th + ")" + "<br>" +
                    "<span style='color:#007acc;'>Detail : </span> " + _ListJobPlanning.Detail.Replace(Environment.NewLine, "<br>") + "<br>" +
                    "<span style='color:#007acc;'>Require Date : </span> " + _ListJobPlanning.RequireDate + "<br>" +
                    "<span style='color:#007acc;'>Start Date : </span> " + _ListJobPlanning.PlanStartDate + "<br>" +
                    "<span style='color:#007acc;'>Finish Date : </span> " + _ListJobPlanning.PlanFinishDate + "<br>" +
                    "<span style='color:#007acc;'>Support By : </span> " + _ListJobPlanning.AssignToName + "<br>";
                sm.SendEMailTo("swadmin@meyer-mil.com", titleEmail, QueryRequest.StrEmailPlannerAndStaff(_ListJobPlanning.AssignTo), "", "", subject, content2, true, "");
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
        public JsonResult SetReWorkJob(ParameterJobRework cc)
        {
            bool status = false;
            string msg = string.Empty;

            ITCContext _dbITC = new ITCContext();
            MILAuthContext _dbMILAuth = new MILAuthContext();
            try
            {
                status = true;
                msg = "Successful";
                JobRequest _ListJobPlanning = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == cc.Id).FirstOrDefault();
                JobReqBody queryJobReqBody = _dbITC.JobReqBody.Where(w => w.Id == _ListJobPlanning.Id).FirstOrDefault();
                queryJobReqBody.Status = 12;

                _dbITC.JobRework.Add(new JobRework
                {
                    Reason = cc.Reason.Replace("\n", Environment.NewLine),
                    WorkOrder_Id = cc.Id
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
    }
}