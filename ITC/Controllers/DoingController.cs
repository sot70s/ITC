using ITC.Models;
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
    public class DoingController : Controller
    {
        public ActionResult Index()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ViewBag.BindDDlWorkOrder = QueryRequest.ListJobPlanning().Where(w => w.AssignTo == emp_no && (w.Status == 5 && w.StatusWorkOrder == true)).OrderByDescending(o => o.WoNo).ToList();
            ViewBag.BindDDlWorkOrderDR = QueryRequest.ListJobPlanning().Where(w => w.AssignTo == emp_no && ((w.Status == 4 && w.StatusWorkOrder == false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13)).GroupBy(g => g.WoNo).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetJobPlanning()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();

            TableJobPlanning data = new TableJobPlanning()
            {
                data = QueryRequest.ListJobPlanning().Where(w => w.AssignTo == emp_no && w.Status == 4 && w.StatusWorkOrder == true).OrderByDescending(o => o.CreateDate).ThenByDescending(t => t.PriorityNo).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobPlanningInfo(int id)
        {
            List<JobRequest> ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == id).ToList();
            return Json(ListJobRequest, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetJobPlanning(ParameterJobPlanning cc)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();

            try
            {
                if (cc.PlanStartDate == null || cc.PlanFinishDate == null || cc.RootCause == null || cc.Solution == null)
                {
                    status = false;
                    msg = "One or more fields have _blank";
                }
                else
                {
                    status = true;
                    msg = "Successful";
                    List<JobPlanning> query = _dbITC.JobPlanning.Where(w => w.JobReqBody_Id == cc.Id && w.WorkOrder_Id == cc.WorkOrder_Id).ToList();

                    if (query.Count() > 0)
                    {
                        query[0].PlanStartDate = Convert.ToDateTime(cc.PlanStartDate);
                        query[0].PlanFinishDate = Convert.ToDateTime(cc.PlanFinishDate);
                        query[0].RootCause = cc.RootCause.Replace("\n", Environment.NewLine);
                        query[0].Solution = cc.Solution.Replace("\n", Environment.NewLine);
                    }
                    else
                    {
                        _dbITC.JobPlanning.Add(new JobPlanning
                        {
                            JobReqBody_Id = cc.Id,
                            PlanStartDate = Convert.ToDateTime(cc.PlanStartDate),
                            PlanFinishDate = Convert.ToDateTime(cc.PlanFinishDate),
                            RootCause = cc.RootCause.Replace("\n", Environment.NewLine),
                            Solution = cc.Solution.Replace("\n", Environment.NewLine),
                            WorkOrder_Id = cc.WorkOrder_Id
                        });
                    }
                }
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpPut]
        public JsonResult SendJobPlanning(int id)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            JobReqBody query = _dbITC.JobReqBody.Where(w => w.Id == id).FirstOrDefault();
            try
            {
                status = true;
                msg = "Successful";
                query.Status = 5;
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
        public JsonResult GetJobOnHand()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();

            TableJobPlanning data = new TableJobPlanning()
            {
                data = QueryRequest.ListJobPlanning().Where(w => w.AssignTo == emp_no && w.Status == 5 && w.StatusWorkOrder == true).OrderByDescending(o => o.WoNo).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobOnHandInfo(int id)
        {
            List<JobRequest> ListJobRequest = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == id).ToList();
            return Json(ListJobRequest, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult JobDaily()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ViewBag.BindDDlWorkOrder = QueryRequest.ListJobPlanning().Where(w => w.AssignTo == emp_no && (w.Status == 5 && w.StatusWorkOrder == true)).OrderByDescending(o => o.WoNo).ToList();
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetJobDaily(ParameterJobDaily cc)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> ListJobPlanning = QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == cc.WorkOrder_Id).ToList();
            List<JobRequest> ListJobDaily = QueryRequest.ListJobDaily().Where(w => w.WorkOrder_Id == cc.WorkOrder_Id && Convert.ToDateTime(w.DailyDate) == Convert.ToDateTime(cc.DailyDate) && w.StatusWorkOrder == true).ToList();
            return Json(new { objPlanning = ListJobPlanning, objDaily = ListJobDaily });
        }

        [HttpPost]
        public JsonResult SetJobDaily(ParameterJobDaily cc)
        {
            bool status = false;
            string msg = string.Empty;
            ITCContext _dbITC = new ITCContext();

            if (cc.WorkOrder_Id == 0 || cc.Status == 0 || cc.TimeStart == null || cc.TimeStop == null || cc.RootCause == null || cc.Solution == null)
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
                    WorkOrders _WorkOrder = _dbITC.WorkOrders.Where(w => w.Id == cc.WorkOrder_Id).FirstOrDefault();
                    JobReqBody _JobReqBody = _dbITC.JobReqBody.Where(w => w.Id == _WorkOrder.JobReqBody_Id).FirstOrDefault();
                    JobPlanning _JobPlanning = _dbITC.JobPlanning.Where(w => w.JobReqBody_Id == _WorkOrder.JobReqBody_Id && w.WorkOrder_Id == cc.WorkOrder_Id).FirstOrDefault();
                    var hourStart = Convert.ToDateTime(cc.TimeStart).Hour;
                    var minuteStart = Convert.ToDateTime(cc.TimeStart).Minute;
                    var hourEnd = Convert.ToDateTime(cc.TimeStop).Hour;
                    var minuteEnd = Convert.ToDateTime(cc.TimeStop).Minute;
                    if (cc.Status == 3)
                    {
                        _JobReqBody.Status = 6;
                    }
                    _WorkOrder.Progress = cc.Progress;
                    _JobPlanning.RootCause = cc.RootCause.Replace("\n", Environment.NewLine);
                    _JobPlanning.Solution = cc.Solution.Replace("\n", Environment.NewLine);
                    _JobPlanning.Note = cc.Note.Replace("\n", Environment.NewLine);
                    _dbITC.JobDaily.Add(new JobDaily
                    {
                        TimeStart = Convert.ToDateTime(cc.DailyDate).AddHours(hourStart).AddMinutes(minuteStart),
                        TimeStop = Convert.ToDateTime(cc.DailyDate).AddHours(hourEnd).AddMinutes(minuteEnd),
                        Status = cc.Status,
                        WorkOrder_Id = cc.WorkOrder_Id,
                        DailyDate = Convert.ToDateTime(cc.DailyDate)
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

        [HttpGet]
        public JsonResult GetReasonJobRework(int WorkOrder_Id)
        {
            return Json(QueryRequest.ListJobRework().Where(w => w.WorkOrder_Id == WorkOrder_Id).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCalendarDaily()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            return Json(QueryRequest.ListCalendarDaily(emp_no), JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DelJobDaily(ParameterJobDaily cc)
        {
            ITCContext _dbITC = new ITCContext();
            JobDaily query = _dbITC.JobDaily.FirstOrDefault(s => s.Id == cc.JobDaily_Id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.JobDaily.Remove(query);
            _dbITC.SaveChanges();

            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult GetDailyReport(ParameterDailyReport cc)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            List<DailyReport> ListDailyReport = QueryRequest.ListJobDaily().Where(w => w.AssignTo == emp_no && ((w.Status == 4 && w.StatusWorkOrder == false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13))
                                   .GroupBy(g => new
                                   {
                                       g.Id,
                                       g.WorkOrder_Id,
                                       g.WoNo,
                                       g.Rework,
                                       g.Progress,
                                       g.Priority,
                                       g.Status,
                                       g.PlanFinishDate,
                                       g.CriticalDate,
                                       g.CriticalPercent,
                                       g.StatusWorkOrder
                                   }, (key, group) => new DailyReport
                                   {
                                       Id = key.Id,
                                       WorkOrder_Id = key.WorkOrder_Id,
                                       WoNo = key.WoNo,
                                       Rework = key.Rework,
                                       Progress = key.Progress,
                                       Priority = key.Priority,
                                       Status = key.Status,
                                       LeadTime = (DateTime.Now - Convert.ToDateTime(key.PlanFinishDate)).Days,
                                       CriticalDate = key.CriticalDate,
                                       CriticalPercent = key.CriticalPercent,
                                       StatusWorkOrder = key.StatusWorkOrder
                                   }).OrderByDescending(o => o.WoNo).ThenByDescending(t => t.Rework).ToList();


            if (cc.WorkOrder != "0")
            {
                ListDailyReport = ListDailyReport.Where(w => w.WoNo == cc.WorkOrder).ToList();
            }


            TableDailyReport data = new TableDailyReport()
            {
                data = ListDailyReport
            };
            return Json(data, JsonRequestBehavior.AllowGet);
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
                email_to = QueryRequest.StrEmailRequestor(cc.JobReqBody_Id);
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