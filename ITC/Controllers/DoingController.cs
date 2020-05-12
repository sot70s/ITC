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
                data = QueryRequest.ListJobPlanning().Where(w => w.AssignTo == emp_no && w.Status == 4 && w.StatusWorkOrder == true).ToList()
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

            if (cc.WorkOrder_Id == 0 || cc.Status == 0 || cc.TimeStart == null || cc.TimeStop == null || cc.WorkDetail == null || cc.RootCause == null || cc.Solution == null)
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
                    _dbITC.JobDaily.Add(new JobDaily
                    {
                        TimeStart = Convert.ToDateTime(cc.DailyDate).AddHours(hourStart).AddMinutes(minuteStart),
                        TimeStop = Convert.ToDateTime(cc.DailyDate).AddHours(hourEnd).AddMinutes(minuteEnd),
                        WorkDetail = cc.WorkDetail.Replace("\n", Environment.NewLine),
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
            List<DailyReport> ListDailyReport = QueryRequest.ListJobDaily().Where(w => w.AssignTo == emp_no && ((w.Status == 4 && w.StatusWorkOrder ==false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13))
                                   .GroupBy(g => new
                                   {
                                       g.WorkOrder_Id,
                                       g.WoNo,
                                       g.Rework,
                                       g.Progress,
                                       g.Priority,
                                       g.Status,
                                       g.StatusWorkOrder
                                   }, (key, group) => new DailyReport
                                   {
                                       WorkOrder_Id = key.WorkOrder_Id,
                                       WoNo = key.WoNo,
                                       Rework = key.Rework,
                                       Progress = key.Progress,
                                       Priority = key.Priority,
                                       Status = key.Status,
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
    }
}