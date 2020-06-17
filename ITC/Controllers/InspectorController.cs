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
    public class InspectorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.BindDDlWorkOrder = QueryRequest.ListJobPlanning().Where(w => (w.SectionType.StartsWith("IS") || w.SectionType.StartsWith("IT")) && ((w.Status == 4 && w.StatusWorkOrder == false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13)).GroupBy(g => g.WoNo).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();
            ViewBag.BindDDLAssignTo = QueryMisFlow.ListMisFlow().Where(w => (w.Division.StartsWith("IS") || w.Division.StartsWith("IT")) && w.JobType == "Staff")
                .GroupBy(g => new
                {
                    g.EmployeeNo,
                    g.EMPLOYEE_NAME
                }, (key,group) => new MisFlowJoinEmployee
                {
                    EmployeeNo = key.EmployeeNo,
                    EMPLOYEE_NAME = key.EMPLOYEE_NAME
                }).ToList();
            ViewBag.BindYear = QueryRequest.ListJobPlanning().Where(w => (w.Status > 2 && (w.Status != 10))).GroupBy(g => Convert.ToDateTime(g.CreateDate).ToString("yyyy")).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetTrackingJob(ParameterDailyReport cc)
        {
            List<DailyReport> ListDailyReport = QueryRequest.ListJobDailyTracking().Where(w => (w.SectionType.StartsWith("IS") || w.SectionType.StartsWith("IT")) && ((w.Status == 4 && w.StatusWorkOrder == false) || w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13))
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
            MP2Context _dbMP2 = new MP2Context();
            List<JobRequest> ListJobRequest = (from jp in QueryRequest.ListJobPlanning().Where(w => w.WorkOrder_Id == id).ToList()
                                               join eq in _dbMP2.EQUIP.ToList()
                                               on jp.Equipment equals eq.EQNUM into joined
                                               from j in joined.DefaultIfEmpty()
                                               select new JobRequest
                                               {
                                                   Id = jp.Id,
                                                   WorkRequest = jp.WorkRequest,
                                                   Equipment = jp.Equipment,
                                                   Description = jp.Description,
                                                   Line = jp.Line,
                                                   Symptom = jp.Symptom,
                                                   SymptomName_Th = jp.SymptomName_Th,
                                                   StandardDate = jp.StandardDate,
                                                   CriticalDate = jp.CriticalDate,
                                                   CriticalPercent = jp.CriticalPercent,
                                                   RequireDate = jp.RequireDate,
                                                   CreateDate = jp.CreateDate,
                                                   Priority = jp.Priority,
                                                   PriorityNo = jp.PriorityNo,
                                                   Status = jp.Status,
                                                   Detail = jp.Detail,
                                                   DecisionType = jp.DecisionType,
                                                   SectionType = jp.SectionType,
                                                   Suggestion = jp.Suggestion,
                                                   Requestor = jp.Requestor,
                                                   RequestorName = jp.RequestorName,
                                                   Responsible = jp.Responsible,
                                                   ResponsibleName = jp.ResponsibleName,
                                                   AssignTo = jp.AssignTo,
                                                   AssignToName = jp.AssignToName,
                                                   WorkOrder_Id = jp.WorkOrder_Id,
                                                   WoNo = jp.WoNo,
                                                   StatusWorkOrder = jp.StatusWorkOrder,
                                                   Rework = jp.Rework,
                                                   Progress = jp.Progress,
                                                   PlanStartDate = jp.PlanStartDate,
                                                   PlanFinishDate = jp.PlanFinishDate,
                                                   RootCause = jp.RootCause,
                                                   Solution = jp.Solution,
                                                   Note = jp.Note,
                                                   SUBLOCATION1 = (j == null) ? "" : j.SUBLOCATION1,
                                                   SUBLOCATION2 = (j == null) ? "" : j.SUBLOCATION2,
                                                   SUBLOCATION3 = (j == null) ? "N/A" : j.SUBLOCATION1 + " / " + j.SUBLOCATION2
                                               }).ToList();
                
            return Json(ListJobRequest, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInfoDailyReport(ParameterJobDaily cc)
        {
            ITCContext _dbITC = new ITCContext();
            List<DailyReport> _ListQueryRequest = QueryRequest.ListDailyReport().Where(w => w.WorkOrder_Id == cc.id).ToList();
            var total = "";
            if (_ListQueryRequest.Count() > 0)
            {
                TimeSpan duration = new TimeSpan(0, 0, 0, 0);
                for (int i = 0; i < _ListQueryRequest.Count(); i++)
                {
                    var item = _ListQueryRequest[i];
                    duration = DateTime.Parse(item.TimeStop).Subtract(DateTime.Parse(item.TimeStart)) + duration;
                }

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
            }
            else
            {
                total = "N/A";
                _ListQueryRequest = _dbITC.WorkOrders.Where(w => w.Id == cc.id).Select(s => new DailyReport
                {
                    WorkOrder_Id = s.Id,
                    WoNo = s.WoNo,
                    Rework = (s.Rework.ToString().Length == 1) ? "0" + s.Rework.ToString() : s.Rework.ToString(),
                }).ToList();
            }

            return Json(new { obj = _ListQueryRequest, working = total });
        }

        [HttpGet]
        public JsonResult GetReasonJobRework(int WorkOrder_Id)
        {
            return Json(QueryRequest.ListJobRework().Where(w => w.WorkOrder_Id == WorkOrder_Id).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTeamSupport()
        {
            TableMisFlowJoinEmployee data = new TableMisFlowJoinEmployee()
            {
                data = QueryMisFlow.ListMisFlow().Where(w => (w.Division.StartsWith("IS") || w.Division.StartsWith("IT")) && w.JobType == "Staff").OrderBy(o => o.Division).ThenBy(t => t.EmployeeNo).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
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
        public JsonResult ChartStatusOnHand(string year)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            List<JobStatusOnHandChart> objStatus = new List<JobStatusOnHandChart>();
            JobStatusOnHandChart itemStatus;
            List<JobRequest> _ListJobRequest = new List<JobRequest>();

            if (year != "0")
            {
                _ListJobRequest = QueryRequest.ListJobPlanning().Where(w => (w.Status > 2 && (w.Status != 10)) && Convert.ToDateTime(w.CreateDate).ToString("yyyy") == year)
                                .Select(s => new JobRequest
                                {
                                    AssignToName = s.AssignToName,
                                    StatusStr = (s.Status == 9 && s.StatusWorkOrder == true) ? "COMPLETE" : (((s.Status == 6 || s.Status == 7 || s.Status == 9) && s.StatusWorkOrder == false) || s.Status == 12 || s.Status == 13) ? "REWORKED" : (s.Status == 11) ? "REJECTED" : "PROCEED"
                                }).ToList();
            }
            else
            {
                _ListJobRequest = QueryRequest.ListJobPlanning().Where(w => (w.Status > 2 && (w.Status != 10)))
                .Select(s => new JobRequest
                {
                    AssignToName = s.AssignToName,
                    StatusStr = (s.Status == 9 && s.StatusWorkOrder == true) ? "COMPLETE" : (((s.Status == 6 || s.Status == 7 || s.Status == 9) && s.StatusWorkOrder == false) || s.Status == 12 || s.Status == 13) ? "REWORKED" : (s.Status == 11) ? "REJECTED" : "PROCEED"
                }).ToList();
            }

            _ListJobRequest = _ListJobRequest.GroupBy(g => new
            {
                g.StatusStr,
                g.AssignToName
            }
                                , (key, group) => new JobRequest
                                {
                                    AssignToName = key.AssignToName,
                                    StatusStr = key.StatusStr,
                                    CountStatus = group.Count()
                                }).ToList();

            var item = _ListJobRequest.ToList()
                    .GroupBy(g => g.AssignToName)
                    .Select(s => new
                    {
                        AssignToName = s.Key,
                        Rejected = s.Where(g => g.StatusStr == "REJECTED").Select(s2 => s2.CountStatus).FirstOrDefault(),
                        Reworked = s.Where(g => g.StatusStr == "REWORKED").Select(s2 => s2.CountStatus).FirstOrDefault(),
                        Proceed = s.Where(g => g.StatusStr == "PROCEED").Select(s2 => s2.CountStatus).FirstOrDefault(),
                        Complete = s.Where(g => g.StatusStr == "COMPLETE").Select(s2 => s2.CountStatus).FirstOrDefault()
                    }).ToList();

            for (int i = 0; i < item.Count(); i++)
            {
                itemStatus = new JobStatusOnHandChart();
                itemStatus.AssignToName = item[i].AssignToName;
                itemStatus.Rejected = item[i].Rejected;
                itemStatus.Reworked = item[i].Reworked;
                itemStatus.Proceed = item[i].Proceed;
                itemStatus.Complete = item[i].Complete;
                objStatus.Add(itemStatus);
            }

            return Json(new { obj = objStatus.ToList() });
        }


        public void chart()
        {
            List<Chart> Obj = new List<Chart>();
            Chart item;
            string[] Location = { "L1", "L2", "L3", "L4" };
            int[] CountType = { 1, 2, 3, 4, 5 };
            for (int i = 0; i < Location.Count(); i++)
            {
                item = new Chart();
                string[] EquipType = { "EQ1", "EQ2", "EQ3", "EQ4", "EQ5" }; // ใช้ Location ในการ Where

                for (int j = 0; j < EquipType.Count(); j++)
                {
                    item.Location = "";
                    item.EquipType[j] = EquipType[j];
                    item.CountType[j] = CountType[j];
                    //L1 : "",
                    //EQ1 : "",
                    //EQ2 : "",
                    //EQ3 : "",
                    //EQ4 : "",
                }
                Obj.Add(item);
            }
        }
    }

    public class Chart
    {
        public string Location { get; set; }
        public string[] EquipType { get; set; }
        public int[] CountType { get; set; }
    }
}