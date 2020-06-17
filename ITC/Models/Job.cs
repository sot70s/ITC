using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace ITC.Models
{
    public class ParameterJobReqBody
    {
        public int Id { get; set; }
        public string WorkRequest { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public string Symptom { get; set; }
        public string Detail { get; set; }
        public string RequireDate { get; set; }
        public int Status { get; set; }
        public int JobReqHeader_Id { get; set; }
        public string Suggestion { get; set; }
        public string Responsible { get; set; }
        public string AssignTo { get; set; }
        public string FieldsDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ParameterJobPlanning
    {
        public int Id { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanFinishDate { get; set; }
        public string RootCause { get; set; }
        public string Solution { get; set; }
        public int WorkOrder_Id { get; set; }
        public string Reason { get; set; }
    }

    public class ParameterJobDaily
    {
        public int id { get; set; }
        public int JobDaily_Id { get; set; }
        public int WorkOrder_Id { get; set; }
        public string WorkOrder { get; set; }
        public string DailyDate { get; set; }
        public string TimeStart { get; set; }
        public string TimeStop { get; set; }
        public string WorkDetail { get; set; }
        public string RootCause { get; set; }
        public string Solution { get; set; }
        public int Status { get; set; }
        public int Progress { get; set; }
        public string Note { get; set; }
    }

    public class ParameterDailyReport
    {
        public string WorkOrder { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AssignTo { get; set; }
    }

    public class ParameterTrackingJob
    {
        public int Id { get; set; }
        public string WorkOrder { get; set; }
        public string Reason { get; set; }
    }

    public class ParameterJobRework
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int JobReqBody { get; set; }
        public int WorkOrder_Id { get; set; }
    }

    public class ParameterSetFeed
    {
        public int JobReqBody_Id { get; set; }
        public int TypeFeed { get; set; }
        public string TitleComment { get; set; }
        public int TitleReply { get; set; }
        public string Comment { get; set; }
        public int Id { get; set; }
        public string WorkRequest { get; set; }
        public string Equipment { get; set; }
    }

    [Table("JobReqHeader")]
    public class JobReqHeader
    {
        [Key]
        public int Id { get; set; }
        public string WorkRequest { get; set; }
        public DateTime CreateDate { get; set; }
        public string Requestor { get; set; }
    }

    [Table("JobReqBody")]
    public class JobReqBody
    {
        [Key]
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public string Symptom { get; set; }
        public string Detail { get; set; }
        public int Line { get; set; }
        public DateTime RequireDate { get; set; }
        public int Status { get; set; }
        public string WorkRequest { get; set; }
        public string Responsible { get; set; }
        public string AssignTo { get; set; }
    }

    [Table("SolutionSuggestion")]
    public class SolutionSuggestion
    {
        [Key]
        public int Id { get; set; }
        public string Solution { get; set; }
        public int JobReqBody_Id { get; set; }
    }

    [Table("JobPlanning")]
    public class JobPlanning
    {
        [Key]
        public int Id { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanFinishDate { get; set; }
        public string RootCause { get; set; }
        public string Solution { get; set; }
        public int JobReqBody_Id { get; set; }
        public int WorkOrder_Id { get; set; }
        public string Note { get; set; }
    }

    [Table("WorkOrders")]
    public class WorkOrders
    {
        [Key]
        public int Id { get; set; }
        public string WoNo { get; set; }
        public DateTime CreateDate { get; set; }
        public int JobReqBody_Id { get; set; }
        public int Rework { get; set; }
        public bool Status { get; set; }
        public int Progress { get; set; }
    }

    [Table("JobDaily")]
    public class JobDaily
    {
        [Key]
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeStop { get; set; }
        public string WorkDetail { get; set; }
        public int Status { get; set; }
        public int WorkOrder_Id { get; set; }
        public DateTime DailyDate { get; set; }
        public int Progress { get; set; }
    }

    [Table("JobRework")]
    public class JobRework
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
        public int WorkOrder_Id { get; set; }
    }

    [Table("JobUnAccept")]
    public class JobUnAccept
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
        public int WorkOrder_Id { get; set; }
    }

    [Table("JobComment")]
    public class JobComment
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int JobReqBody_Id { get; set; }
    }

    [Table("JobReply")]
    public class JobReply
    {
        [Key]
        public int Id { get; set; }
        public string Reply { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int JobComment_Id { get; set; }
        public int JobReqBody_Id { get; set; }
    }

    public class Progress
    {
        public int SumProgress { get;set;}
    }

    public class JobComments
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public int JobComment_Id { get; set; }
        public string WorkRequest { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public int JobReqBody_Id { get; set; }
        public List<JobReplies> ObjJobReply { get; set; }
    }

    public class JobReplies
    {
        public int Id { get; set; }
        public string Reply { get; set; }
        public string CreateBy { get; set; }
        public string EmployeeName { get; set; }
        public string CreateDate { get; set; }
        public int JobComment_Id { get; set; }
        public int JobReqBody_Id { get; set; }
    }

    public class JobRequest
    {
        public int Id { get; set; }
        public string WorkRequest { get; set; }
        public string Requestor { get; set; }
        public string RequestorName { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public string Symptom { get; set; }
        public string SymptomName_Th { get; set; }
        public string Detail { get; set; }
        public string Line { get; set; }
        public string RequireDate { get; set; }
        public int Status { get; set; }
        public string DecisionType { get; set; }
        public string SectionType { get; set; }
        public int Score { get; set; }
        public string SectionCode { get; set; }
        public string Email { get; set; }
        public string Responsible { get; set; }
        public string Priority { get; set; }
        public string PriorityNo { get; set; }
        public string ResponsibleName { get; set; }
        public string Suggestion { get; set; }
        public string AssignTo { get; set; }
        public string AssignToName { get; set; }
        public int WorkOrder_Id { get; set; }
        public string WoNo { get; set; }
        public string Solution { get; set; }
        public bool StatusWorkOrder { get; set; }
        public string Rework { get; set; }
        public string CreateDate { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanFinishDate { get; set; }
        public string RootCause { get; set; }
        public int Progress { get; set; }
        public int JobDaily_Id { get; set; }
        public string TimeStart { get; set; }
        public string TimeStop { get; set; }
        public string DailyDate { get; set; }
        public string WorkDetail { get; set; }
        public string StatusWorkOrderStr { get; set; }
        public string Reason { get; set; }
        public int CountStatus { get; set; }
        public string StatusStr { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveDate { get; set; }
        public string Note { get; set; }
        public int StandardDate { get; set; }
        public int CriticalDate { get; set; }
        public int CriticalPercent { get; set; }
        public string SUBLOCATION1 { get; set; }
        public string SUBLOCATION2 { get; set; }
        public string SUBLOCATION3 { get; set; }
    }

    public class JobGradeScore
    {
        public int Id { get; set; }
        public string GradeCode { get; set; }
        public int Score { get; set; }
        public string Requestor { get; set; }
    }

    public class JobSymptomScore
    {
        public int Id { get; set; }
        public string Symptom { get; set; }
        public int Score { get; set; }
    }

    public class PriorityScore
    {
        public int Id { get; set; }
        public string Priority { get; set; }
        public string PriorityNo { get; set; }
        public int Score { get; set; }
    }

    public class CalendarDaily
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
    }

    public class DailyReport
    {
        public int Id { get; set; }
        public int WorkOrder_Id { get; set; }
        public string WoNo { get; set; }
        public string Priority { get; set; }
        public string Rework { get; set; }
        public int Progress { get; set; }
        public string WorkDetail { get; set; }
        public string StatusWorkOrderStr { get; set; }
        public string DailyDate { get; set; }
        public string TimeStart { get; set; }
        public string TimeStop { get; set; }
        public string AssignToName { get; set; }
        public string AssignTo { get; set; }
        public int Status { get; set; }
        public bool StatusWorkOrder { get; set; }
        public int StandardDate { get; set; }
        public int CriticalDate { get; set; }
        public int CriticalPercent { get; set; }
        public int LeadTime { get; set; }
    }

    public class Year
    {
        public string year { get; set; }
    }

    public class TableJobReqHeader
    {
        public List<JobReqHeader> data { get; set; }
    }

    public class TableJobRequest
    {
        public List<JobRequest> data { get; set; }
    }

    public class TableJobResponsible
    {
        public List<JobRequest> data { get; set; }
    }

    public class TableJobPlanning
    {
        public List<JobRequest> data { get; set; }
    }

    public class TableDailyReport
    {
        public List<DailyReport> data { get; set; }
    }

    public class QueryRequest
    {
        public static List<JobRequest> ListJobWorkRequest()
        {
            ITCContext _dbITC = new ITCContext();

            List<JobRequest> query = (from jrh in _dbITC.JobReqHeader.ToList()
                                      join jrb in _dbITC.JobReqBody.ToList()
                                      on jrh.WorkRequest equals jrb.WorkRequest into joined
                                      from j in joined.DefaultIfEmpty()
                                      select new JobRequest
                                      {
                                          Id = (j == null) ? 0 : j.Id,
                                          WorkRequest = jrh.WorkRequest,
                                          Requestor = jrh.Requestor,
                                          Equipment = (j == null) ? "" : j.Equipment,
                                          Description = (j == null) ? "" : j.Description,
                                          Symptom = (j == null) ? "" : j.Symptom,
                                          Responsible = (j == null) ? "" : j.Responsible,
                                          Detail = (j == null) ? "" : j.Detail,
                                          RequireDate = (j == null) ? "" : j.RequireDate.ToString(),
                                          Status = (j == null) ? 0 : j.Status,
                                          AssignTo = (j == null) ? "" : j.AssignTo,
                                          Line = (j == null) ? "0" : (j.Line.ToString().Length == 1) ? "0" + j.Line.ToString() : j.Line.ToString()
                                      }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobWorkRequestInfo()
        {
            ITCContext _dbITC = new ITCContext();

            List<JobRequest> query = (from jrh in _dbITC.JobReqHeader.ToList()
                                      join jrb in _dbITC.JobReqBody.ToList()
                                      on jrh.WorkRequest equals jrb.WorkRequest into joined
                                      from j in joined.DefaultIfEmpty()
                                      select new JobRequest
                                      {
                                          Id = (j == null) ? 0 : j.Id,
                                          WorkRequest = jrh.WorkRequest,
                                          Requestor = jrh.Requestor,
                                          Equipment = (j == null) ? "" : j.Equipment,
                                          Description = (j == null) ? "" : j.Description,
                                          Symptom = (j == null) ? "" : j.Symptom,
                                          Responsible = (j == null) ? "" : j.Responsible,
                                          Detail = (j == null) ? "" : j.Detail,
                                          RequireDate = (j == null) ? "" : j.RequireDate.ToString(),
                                          Status = (j == null) ? 0 : j.Status,
                                          AssignTo = (j == null) ? "" : j.AssignTo,
                                          Line = (j == null) ? "0" : (j.Line.ToString().Length == 1) ? "0" + j.Line.ToString() : j.Line.ToString()
                                      }).ToList();

            query = query.ToList().Join(_dbITC.Symptom.ToList(),
                jhb => jhb.Symptom,
                sm => sm.SymptomName,
                (jhb, sm) => new JobRequest
                {
                    Id = jhb.Id,
                    WorkRequest = jhb.WorkRequest,
                    Requestor = jhb.Requestor,
                    Equipment = jhb.Equipment,
                    Description = jhb.Description,
                    Symptom = jhb.Symptom,
                    SymptomName_Th = sm.SymptomName_Th,
                    Responsible = jhb.Responsible,
                    Detail = jhb.Detail,
                    RequireDate = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(jhb.RequireDate)),
                    Status = jhb.Status,
                    AssignTo = jhb.AssignTo,
                    Line = jhb.Line
                }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobRequest(string work_request)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = _dbITC.JobReqHeader.Where(w => w.WorkRequest == work_request).ToList().Join(_dbITC.JobReqBody.ToList(),
               jh => jh.WorkRequest,
               jb => jb.WorkRequest,
               (jh, jb) => new JobRequest
               {
                   Id = jb.Id,
                   WorkRequest = jh.WorkRequest,
                   Requestor = jh.Requestor,
                   Equipment = jb.Equipment,
                   Description = jb.Description,
                   Symptom = jb.Symptom,
                   Detail = jb.Detail,
                   RequireDate = jb.RequireDate.ToString(),
                   Status = jb.Status
               }).Join(_dbITC.Symptom.ToList(),
               jhb => jhb.Symptom,
               sm => sm.SymptomName,
               (jhb, sm) => new JobRequest
               {
                   Id = jhb.Id,
                   WorkRequest = jhb.WorkRequest,
                   Requestor = jhb.Requestor,
                   Equipment = jhb.Equipment,
                   Description = jhb.Description,
                   Symptom = jhb.Symptom,
                   SymptomName_Th = sm.SymptomName_Th,
                   Detail = jhb.Detail,
                   RequireDate = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(jhb.RequireDate)),
                   Status = jhb.Status
               }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobRequest()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = _dbITC.JobReqHeader.ToList().Join(_dbITC.JobReqBody.ToList(),
               jh => jh.WorkRequest,
               jb => jb.WorkRequest,
               (jh, jb) => new JobRequest
               {
                   Id = jb.Id,
                   WorkRequest = jh.WorkRequest,
                   Requestor = jh.Requestor,
                   CreateDate = jh.CreateDate.ToString(),
                   Responsible = jb.Responsible,
                   Line = (jb.Line.ToString().Length == 1) ? "0" + jb.Line.ToString() : jb.Line.ToString(),
                   AssignTo = jb.AssignTo,
                   Equipment = jb.Equipment,
                   Description = jb.Description,
                   Symptom = jb.Symptom,
                   Detail = jb.Detail,
                   RequireDate = jb.RequireDate.ToString(),
                   Status = jb.Status
               }).Join(_dbITC.Symptom.ToList(),
               jhb => jhb.Symptom,
               sm => sm.SymptomName,
               (jhb, sm) => new JobRequest
               {
                   Id = jhb.Id,
                   WorkRequest = jhb.WorkRequest,
                   Requestor = jhb.Requestor,
                   CreateDate = jhb.CreateDate,
                   Responsible = jhb.Responsible,
                   Line = jhb.Line,
                   AssignTo = jhb.AssignTo,
                   Equipment = jhb.Equipment,
                   Description = jhb.Description,
                   Symptom = jhb.Symptom,
                   SymptomName_Th = sm.SymptomName_Th,
                   StandardDate = sm.StandardDate,
                   CriticalDate = sm.CriticalDate,
                   CriticalPercent = sm.CriticalPercent,
                   Detail = jhb.Detail,
                   RequireDate = jhb.RequireDate,
                   Status = jhb.Status,
                   DecisionType = sm.DecisionType,
                   SectionType = sm.SectionType,
                   Score = sm.Score
               }).ToList();

            query = (from jhb in query.ToList()
                     join ss in _dbITC.SolutionSuggestion.ToList()
                     on jhb.Id equals ss.JobReqBody_Id into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest
                     {
                         Id = jhb.Id,
                         WorkRequest = jhb.WorkRequest,
                         Requestor = jhb.Requestor,
                         CreateDate = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(jhb.CreateDate)),
                         Responsible = jhb.Responsible,
                         Line = jhb.Line,
                         AssignTo = jhb.AssignTo,
                         Equipment = jhb.Equipment,
                         Description = jhb.Description,
                         Symptom = jhb.Symptom,
                         SymptomName_Th = jhb.SymptomName_Th,
                         StandardDate = jhb.StandardDate,
                         CriticalDate = jhb.CriticalDate,
                         CriticalPercent = jhb.CriticalPercent,
                         Detail = jhb.Detail,
                         RequireDate = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(jhb.RequireDate)),
                         Status = jhb.Status,
                         DecisionType = jhb.DecisionType,
                         SectionType = jhb.SectionType,
                         Score = jhb.Score,
                         Suggestion = (j == null) ? "" : j.Solution,
                     }).ToList();

            query = (from jhb in query.ToList()
                     join jap in _dbITC.JobApprove.ToList()
                     on jhb.Id equals jap.JobReqBody_Id into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest
                     {
                         Id = jhb.Id,
                         WorkRequest = jhb.WorkRequest,
                         Requestor = jhb.Requestor,
                         CreateDate = jhb.CreateDate,
                         Responsible = jhb.Responsible,
                         Line = jhb.Line,
                         AssignTo = jhb.AssignTo,
                         Equipment = jhb.Equipment,
                         Description = jhb.Description,
                         Symptom = jhb.Symptom,
                         SymptomName_Th = jhb.SymptomName_Th,
                         StandardDate = jhb.StandardDate,
                         CriticalDate = jhb.CriticalDate,
                         CriticalPercent = jhb.CriticalPercent,
                         Detail = jhb.Detail,
                         RequireDate = jhb.RequireDate,
                         Status = jhb.Status,
                         DecisionType = jhb.DecisionType,
                         SectionType = jhb.SectionType,
                         Score = jhb.Score,
                         Suggestion = jhb.Suggestion,
                         ApproveBy = (j == null) ? "" : j.ApproveBy,
                         ApproveDate = (j == null) ? "" : String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(j.CreateDate)),
                     }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobReject()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = ListJobRequest().ToList().Join(_dbITC.RejectJobApprover.ToList(),
                                    jr => jr.Id,
                                    rja => rja.JobReqBody_Id,
                                    (jr, rja) => new JobRequest
                                    {
                                        Id = jr.Id,
                                        WorkRequest = jr.WorkRequest,
                                        Requestor = jr.Requestor,
                                        CreateDate = jr.CreateDate,
                                        Responsible = jr.Responsible,
                                        Line = jr.Line,
                                        AssignTo = jr.AssignTo,
                                        Equipment = jr.Equipment,
                                        Description = jr.Description,
                                        Symptom = jr.Symptom,
                                        SymptomName_Th = jr.SymptomName_Th,
                                        StandardDate = jr.StandardDate,
                                        CriticalDate = jr.CriticalDate,
                                        CriticalPercent = jr.CriticalPercent,
                                        Detail = jr.Detail,
                                        RequireDate = jr.RequireDate,
                                        Status = jr.Status,
                                        DecisionType = jr.DecisionType,
                                        SectionType = jr.SectionType,
                                        Score = jr.Score,
                                        Suggestion = jr.Solution,
                                        Reason = rja.Reason
                                    }).ToList();
            return query;
        }

        public static List<JobRequest> ListJobRework()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = ListJobRequest().ToList().Join(_dbITC.WorkOrders.ToList(),
                                    jr => jr.Id,
                                    wo => wo.JobReqBody_Id,
                                    (jr, wo) => new JobRequest
                                    {
                                        Id = jr.Id,
                                        WorkRequest = jr.WorkRequest,
                                        Requestor = jr.Requestor,
                                        CreateDate = jr.CreateDate,
                                        Responsible = jr.Responsible,
                                        Line = jr.Line,
                                        AssignTo = jr.AssignTo,
                                        Equipment = jr.Equipment,
                                        Description = jr.Description,
                                        Symptom = jr.Symptom,
                                        SymptomName_Th = jr.SymptomName_Th,
                                        Detail = jr.Detail,
                                        RequireDate = jr.RequireDate,
                                        Status = jr.Status,
                                        DecisionType = jr.DecisionType,
                                        SectionType = jr.SectionType,
                                        Score = jr.Score,
                                        Suggestion = jr.Solution,
                                        WorkOrder_Id = wo.Id
                                    }).ToList();

            query = query.Join(_dbITC.JobRework.ToList(),
                                    jr => jr.WorkOrder_Id,
                                    rw => rw.WorkOrder_Id,
                                    (jr, rw) => new JobRequest
                                    {
                                        Id = jr.Id,
                                        WorkRequest = jr.WorkRequest,
                                        Requestor = jr.Requestor,
                                        CreateDate = jr.CreateDate,
                                        Responsible = jr.Responsible,
                                        Line = jr.Line,
                                        AssignTo = jr.AssignTo,
                                        Equipment = jr.Equipment,
                                        Description = jr.Description,
                                        Symptom = jr.Symptom,
                                        SymptomName_Th = jr.SymptomName_Th,
                                        Detail = jr.Detail,
                                        RequireDate = jr.RequireDate,
                                        Status = jr.Status,
                                        DecisionType = jr.DecisionType,
                                        SectionType = jr.SectionType,
                                        Score = jr.Score,
                                        Suggestion = jr.Solution,
                                        WorkOrder_Id = jr.WorkOrder_Id,
                                        Reason = rw.Reason
                                    }).ToList();
            return query;
        }

        public static List<JobRequest> ListJobRequestJoinSymptom()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = (from jr in ListJobRequest().ToList()
                                      join emp in QueryPersonnel.ListEmployeeMeyer().ToList()
                                      on jr.Requestor equals emp.EMPLOYEE_NO into joined
                                      from j in joined.DefaultIfEmpty()
                                      select new JobRequest
                                      {
                                          Id = jr.Id,
                                          WorkRequest = jr.WorkRequest,
                                          Equipment = jr.Equipment,
                                          Description = jr.Description,
                                          Line = jr.Line,
                                          Symptom = jr.Symptom,
                                          SymptomName_Th = jr.SymptomName_Th,
                                          StandardDate = jr.StandardDate,
                                          CriticalDate = jr.CriticalDate,
                                          CriticalPercent = jr.CriticalPercent,
                                          Detail = jr.Detail,
                                          RequireDate = jr.RequireDate,
                                          CreateDate = jr.CreateDate,
                                          Status = jr.Status,
                                          DecisionType = jr.DecisionType,
                                          SectionType = jr.SectionType,
                                          Score = jr.Score,
                                          SectionCode = (j == null) ? "" : j.SECTION_CODE,
                                          Suggestion = jr.Suggestion,
                                          Requestor = jr.Requestor,
                                          RequestorName = (j == null) ? "" : j.EMPLOYEE_NAME,
                                          Responsible = jr.Responsible,
                                          ResponsibleName = jr.ResponsibleName,
                                          AssignTo = jr.AssignTo,
                                          ApproveBy = jr.ApproveBy,
                                          ApproveDate = jr.ApproveDate
                                      }).ToList();

            query = (from jr in query.ToList()
                     join emp in QueryPersonnel.ListEmployeeMeyer().ToList()
                     on jr.Responsible equals emp.EMPLOYEE_NO into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest
                     {
                         Id = jr.Id,
                         WorkRequest = jr.WorkRequest,
                         Equipment = jr.Equipment,
                         Description = jr.Description,
                         Line = jr.Line,
                         Symptom = jr.Symptom,
                         SymptomName_Th = jr.SymptomName_Th,
                         StandardDate = jr.StandardDate,
                         CriticalDate = jr.CriticalDate,
                         CriticalPercent = jr.CriticalPercent,
                         Detail = jr.Detail,
                         RequireDate = jr.RequireDate,
                         CreateDate = jr.CreateDate,
                         Status = jr.Status,
                         DecisionType = jr.DecisionType,
                         SectionType = jr.SectionType,
                         Score = jr.Score,
                         SectionCode = jr.SectionCode,
                         Suggestion = jr.Suggestion,
                         Requestor = jr.Requestor,
                         RequestorName = jr.RequestorName,
                         Responsible = jr.Responsible,
                         ResponsibleName = (j == null) ? "" : j.EMPLOYEE_NAME,
                         AssignTo = jr.AssignTo,
                         ApproveBy = jr.ApproveBy,
                         ApproveDate = jr.ApproveDate
                     }).ToList();

            query = (from jr in query.ToList()
                     join emp in QueryPersonnel.ListEmployeeMeyer().ToList()
                     on jr.AssignTo equals emp.EMPLOYEE_NO into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest
                     {
                         Id = jr.Id,
                         WorkRequest = jr.WorkRequest,
                         Equipment = jr.Equipment,
                         Description = jr.Description,
                         Line = jr.Line,
                         Symptom = jr.Symptom,
                         SymptomName_Th = jr.SymptomName_Th,
                         StandardDate = jr.StandardDate,
                         CriticalDate = jr.CriticalDate,
                         CriticalPercent = jr.CriticalPercent,
                         Detail = jr.Detail,
                         RequireDate = jr.RequireDate,
                         CreateDate = jr.CreateDate,
                         Status = jr.Status,
                         DecisionType = jr.DecisionType,
                         SectionType = jr.SectionType,
                         Score = jr.Score,
                         SectionCode = jr.SectionCode,
                         Suggestion = jr.Suggestion,
                         Requestor = jr.Requestor,
                         RequestorName = jr.RequestorName,
                         Responsible = jr.Responsible,
                         ResponsibleName = jr.ResponsibleName,
                         AssignTo = jr.AssignTo,
                         ApproveBy = jr.ApproveBy,
                         ApproveDate = jr.ApproveDate,
                         AssignToName = (j == null) ? "" : j.EMPLOYEE_NAME,
                     }).ToList();

            query = (from jr in query.ToList()
                     join emp in QueryPersonnel.ListEmployeeMeyer().ToList()
                     on jr.ApproveBy equals emp.EMPLOYEE_NO into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest
                     {
                         Id = jr.Id,
                         WorkRequest = jr.WorkRequest,
                         Equipment = jr.Equipment,
                         Description = jr.Description,
                         Line = jr.Line,
                         Symptom = jr.Symptom,
                         SymptomName_Th = jr.SymptomName_Th,
                         StandardDate = jr.StandardDate,
                         CriticalDate = jr.CriticalDate,
                         CriticalPercent = jr.CriticalPercent,
                         Detail = jr.Detail,
                         RequireDate = jr.RequireDate,
                         CreateDate = jr.CreateDate,
                         Status = jr.Status,
                         DecisionType = jr.DecisionType,
                         SectionType = jr.SectionType,
                         Score = jr.Score,
                         SectionCode = jr.SectionCode,
                         Suggestion = jr.Suggestion,
                         Requestor = jr.Requestor,
                         RequestorName = jr.RequestorName,
                         Responsible = jr.Responsible,
                         ResponsibleName = jr.ResponsibleName,
                         AssignTo = jr.AssignTo,
                         AssignToName = jr.AssignToName,
                         ApproveBy = jr.ApproveBy,
                         ApproveDate = jr.ApproveDate,
                         ApproveByName = (j == null) ? "" : j.EMPLOYEE_NAME
                     }).ToList();

            MP2Context _dbMP2 = new MP2Context();
            query = (from jr in query.ToList()
                     join eq in _dbMP2.EQUIP.ToList()
                     on jr.Equipment equals eq.EQNUM into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest {
                         Id = jr.Id,
                         WorkRequest = jr.WorkRequest,
                         Equipment = jr.Equipment,
                         Description = jr.Description,
                         Line = jr.Line,
                         Symptom = jr.Symptom,
                         SymptomName_Th = jr.SymptomName_Th,
                         StandardDate = jr.StandardDate,
                         CriticalDate = jr.CriticalDate,
                         CriticalPercent = jr.CriticalPercent,
                         Detail = jr.Detail,
                         RequireDate = jr.RequireDate,
                         CreateDate = jr.CreateDate,
                         Status = jr.Status,
                         DecisionType = jr.DecisionType,
                         SectionType = jr.SectionType,
                         Score = jr.Score,
                         SectionCode = jr.SectionCode,
                         Suggestion = jr.Suggestion,
                         Requestor = jr.Requestor,
                         RequestorName = jr.RequestorName,
                         Responsible = jr.Responsible,
                         ResponsibleName = jr.ResponsibleName,
                         AssignTo = jr.AssignTo,
                         AssignToName = jr.AssignToName,
                         ApproveBy = jr.ApproveBy,
                         ApproveDate = jr.ApproveDate,
                         ApproveByName = jr.ApproveByName,
                         SUBLOCATION1 = (j == null) ? "" : j.SUBLOCATION1,
                         SUBLOCATION2 = (j == null) ? "" : j.SUBLOCATION2,
                         SUBLOCATION3 = (j == null) ? "N/A" : j.SUBLOCATION1 + " / " + j.SUBLOCATION2
                     }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobReqReject()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = (from jr in ListJobRequestJoinSymptom().ToList()
                                      join rj in _dbITC.RejectJobApprover.ToList()
                                      on jr.Id equals rj.JobReqBody_Id into joined
                                      from j in joined.DefaultIfEmpty()
                                      select new JobRequest
                                      {
                                          Id = jr.Id,
                                          WorkRequest = jr.WorkRequest,
                                          Equipment = jr.Equipment,
                                          Description = jr.Description,
                                          Line = jr.Line,
                                          Symptom = jr.Symptom,
                                          SymptomName_Th = jr.SymptomName_Th,
                                          Detail = jr.Detail,
                                          RequireDate = jr.RequireDate,
                                          CreateDate = jr.CreateDate,
                                          Status = jr.Status,
                                          DecisionType = jr.DecisionType,
                                          SectionType = jr.SectionType,
                                          Score = jr.Score,
                                          SectionCode = jr.SectionCode,
                                          Suggestion = jr.Suggestion,
                                          Requestor = jr.Requestor,
                                          RequestorName = jr.RequestorName,
                                          Responsible = jr.Responsible,
                                          ResponsibleName = jr.ResponsibleName,
                                          AssignTo = jr.AssignTo,
                                          AssignToName = jr.AssignToName,
                                          ApproveBy = jr.ApproveBy,
                                          ApproveDate = jr.ApproveDate,
                                          ApproveByName = jr.ApproveByName,
                                          Reason = (j == null) ? "" : j.Reason,
                                          SUBLOCATION3 = jr.SUBLOCATION3
                                      }).ToList();
            return query;
        }

        public static List<JobRequest> ListEmailApprover(int id)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = QueryApprover.ListApprover().Join(QueryRequest.ListJobRequestJoinSymptom().Where(w => w.Id == id).ToList(),
                ap => ap.SectionCode,
                jr => jr.SectionCode,
                (ap, jr) => new JobRequest
                {
                    Email = ap.Email
                }).ToList();

            return query;
        }

        public static List<MisFlowJoinEmployee> ListEmailManager(string SectionType)
        {
            ITCContext _dbITC = new ITCContext();
            List<MisFlowJoinEmployee> query = QueryMisFlow.ListMisFlow().Where(w => w.JobType == "Manager" && 
            ((SectionType == "ISD") ? w.Division == "IS" :
            (SectionType == "ISS") ? w.Division == "IS" :
            (SectionType == "ITS") ? w.Division == "IS" :
            w.Division == "IT")).ToList();

            return query;
        }

        public static List<JobRequest> ListEmailResponsible(int id)
        {
            ITCContext _dbITC = new ITCContext();
            MILAuthContext _dbMILAuth = new MILAuthContext();

            List<JobRequest> query = _dbITC.JobReqBody.Where(w => w.Id == id).ToList().Join(_dbMILAuth.Accounts.ToList(),
                                    jrb => jrb.AssignTo,
                                    acc => acc.EmployeeNo,
                                    (jrb, acc) => new JobRequest
                                    {
                                        Email = acc.Email
                                    }).ToList();

            return query;
        }

        public static List<MisFlowJoinEmployee> ListEmailPlanner(int id)
        {
            ITCContext _dbITC = new ITCContext();
            MILAuthContext _dbMILAuth = new MILAuthContext();

            var AssignTo = _dbITC.JobReqBody.Where(w => w.Id == id).ToList()[0].AssignTo;


            List<MisFlowJoinEmployee> query = QueryMisFlow.ListMisFlow().Where(w => w.EmployeeNo == AssignTo && w.JobType == "Staff").ToList();
            List<MisFlowJoinEmployee> queryPlanner = QueryMisFlow.ListMisFlow().Where(w => w.Division == ((query[0].Division == "ISD") ? "IS" : (query[0].Division == "ISS") ? "IS" : "IT") && w.JobType == "Planner").ToList();

            return queryPlanner;
        }

        public static List<JobRequest> ListEmailRequestor(int id)
        {
            ITCContext _dbITC = new ITCContext();
            MILAuthContext _dbMILAuth = new MILAuthContext();

            List<JobRequest> query = _dbITC.JobReqBody.Where(w => w.Id == id).ToList().Join(_dbITC.JobReqHeader.ToList(),
                                    jrb => jrb.WorkRequest,
                                    jrh => jrh.WorkRequest,
                                    (jrb, jrh) => new JobRequest
                                    {
                                        WorkRequest = jrb.WorkRequest,
                                        Requestor = jrh.Requestor
                                    }).ToList();

            query = query.ToList().Join(_dbMILAuth.Accounts.ToList(),
                                    jrb => jrb.Requestor,
                                    acc => acc.EmployeeNo,
                                    (jrb, acc) => new JobRequest
                                    {
                                        Email = acc.Email
                                    }).ToList();

            return query;
        }

        public static string StrEmailRequestor(int id)
        {
            var email = "";
            for (int i = 0; i < ListEmailRequestor(id).Count(); i++)
            {
                var item = ListEmailRequestor(id)[i];
                email += ";" + item.Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailResponsible(int id)
        {
            var email = "";
            for (int i = 0; i < ListEmailResponsible(id).Count(); i++)
            {
                var item = ListEmailResponsible(id)[i];
                email += ";" + item.Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailStaff(int id)
        {
            var email = "";
            for (int i = 0; i < ListEmailResponsible(id).Count(); i++)
            {
                var item = ListEmailResponsible(id)[i];
                email += ";" + item.Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailPlanner(int id)
        {
            var email = "";
            for (int i = 0; i < ListEmailPlanner(id).Count(); i++)
            {
                var item = ListEmailPlanner(id)[i];
                email += ";" + item.Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailPlanner(string emp_no)
        {
            List<MisFlowJoinEmployee> query = QueryMisFlow.ListMisFlow().Where(w => w.EmployeeNo == emp_no && w.JobType == "Staff").ToList();
            List<MisFlowJoinEmployee> queryPlanner = QueryMisFlow.ListMisFlow().Where(w => w.Division == ((query[0].Division == "ISD") ? "IS" : (query[0].Division == "ISS") ? "IS" : "IT") && w.JobType == "Planner").ToList();

            var email = "";
            for (int i = 0; i < queryPlanner.Count(); i++)
            {
                email += ";" + queryPlanner[i].Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailPlannerAndStaff(string emp_no)
        {
            List<MisFlowJoinEmployee> query = QueryMisFlow.ListMisFlow().Where(w => w.EmployeeNo == emp_no && w.JobType == "Staff").ToList();
            List<MisFlowJoinEmployee> queryPlanner = QueryMisFlow.ListMisFlow().Where((w => w.Division == ((query[0].Division == "ISD") ? "IS" : (query[0].Division == "ISS") ? "IS" : "IT") && (w.JobType == "Planner") || (w.EmployeeNo == emp_no && w.JobType == "Staff"))).ToList();

            var email = "";
            for (int i = 0; i < queryPlanner.Count(); i++)
            {
                email += ";" + queryPlanner[i].Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailApprover(int id)
        {
            var email = "";
            List<JobRequest> queryEmailApprover = ListEmailApprover(id);
            for (int i = 0; i < queryEmailApprover.Count(); i++)
            {
                email += ";" + queryEmailApprover[i].Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static string StrEmailManager(string SectionType)
        {
            var email = "";
            List<MisFlowJoinEmployee> queryEmailManager = ListEmailManager(SectionType);
            for (int i = 0; i < queryEmailManager.Count(); i++)
            {
                email += ";" + queryEmailManager[i].Email;
            }
            email = Regex.Replace(email, @"\s", "");
            email = email.Substring(1, email.Length - 1);

            return email;
        }

        public static List<JobGradeScore> ListJobGradeScore()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobGradeScore> queryJobGradeScore = ListJobWorkRequest().Where(w => w.Status >= 2).ToList().Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                jwr => jwr.Requestor,
                emp => emp.EMPLOYEE_NO,
                (jwr, emp) => new JobGradeScore
                {
                    Id = jwr.Id,
                    GradeCode = emp.GRADE_CODE,
                    Requestor = jwr.Requestor
                }).Join(_dbITC.Grade.ToList(),
                jwr => jwr.GradeCode,
                gr => gr.GradeCode,
                (jwr, gr) => new JobGradeScore
                {
                    Id = jwr.Id,
                    Requestor = jwr.Requestor,
                    Score = gr.Score
                }).ToList();

            return queryJobGradeScore;
        }

        public static List<JobSymptomScore> ListJobSymptomScore()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobSymptomScore> query = _dbITC.JobReqBody.Where(w => w.Status >= 2).ToList().Join(_dbITC.Symptom.ToList(),
                jrb => jrb.Symptom,
                sm => sm.SymptomName,
                (jrb, sm) => new JobSymptomScore
                {
                    Id = jrb.Id,
                    Symptom = jrb.Symptom,
                    Score = sm.Score
                }).ToList();

            return query;
        }

        public static List<PriorityScore> ListPriorityScore()
        {
            List<PriorityScore> query = ListJobGradeScore().ToList().Join(ListJobSymptomScore().ToList(),
            gs => gs.Id,
            js => js.Id,
            (gs, js) => new PriorityScore
            {
                Id = gs.Id,
                Priority = (gs.Score + js.Score >= 14) ? "HIGH" : ((gs.Score + js.Score < 14) && (gs.Score + js.Score > 8)) ? "MID" : "LOW",
                PriorityNo = (gs.Score + js.Score >= 14) ? "3" : ((gs.Score + js.Score < 14) && (gs.Score + js.Score > 8)) ? "2" : "1",
                Score = gs.Score + js.Score
            }).ToList();
            return query;
        }

        public static List<JobRequest> ListAssignResponsible()
        {
            ITCContext _dbITC = new ITCContext();
            //List<JobRequest> query = ListJobRequestJoinSymptom().Join(ListPriorityScore().ToList(),
            //    jwr => jwr.Id,
            //    ps => ps.Id,
            //    (jwr, ps) => new JobRequest
            //    {
            //        Id = jwr.Id,
            //        WorkRequest = jwr.WorkRequest,
            //        Equipment = jwr.Equipment,
            //        Description = jwr.Description,
            //        Line = (jwr.Line.Length == 1) ? "0" + jwr.Line : jwr.Line,
            //        Symptom = jwr.Symptom,
            //        SymptomName_Th = jwr.SymptomName_Th,
            //        RequireDate = jwr.RequireDate,
            //        CreateDate = jwr.CreateDate,
            //        Priority = ps.Priority,
            //        Status = jwr.Status,
            //        Detail = jwr.Detail,
            //        DecisionType = jwr.DecisionType,
            //        SectionType = jwr.SectionType,
            //        Suggestion = jwr.Suggestion,
            //        Requestor = jwr.Requestor,
            //        RequestorName = jwr.RequestorName,
            //        Responsible = jwr.Responsible,
            //        ResponsibleName = jwr.ResponsibleName,
            //        AssignTo = jwr.AssignTo,
            //        AssignToName = jwr.AssignToName
            //    }).ToList();

            List<JobRequest> query = (from jwr in ListJobRequestJoinSymptom().ToList()
                                      join ps in ListPriorityScore().ToList()
                                      on jwr.Id equals ps.Id into joined
                                      from j in joined.DefaultIfEmpty()
                                      select new JobRequest
                                      {
                                          Id = jwr.Id,
                                          WorkRequest = jwr.WorkRequest,
                                          Equipment = jwr.Equipment,
                                          Description = jwr.Description,
                                          Line = (jwr.Line.Length == 1) ? "0" + jwr.Line : jwr.Line,
                                          Symptom = jwr.Symptom,
                                          SymptomName_Th = jwr.SymptomName_Th,
                                          StandardDate = jwr.StandardDate,
                                          CriticalDate = jwr.CriticalDate,
                                          CriticalPercent = jwr.CriticalPercent,
                                          RequireDate = jwr.RequireDate,
                                          CreateDate = jwr.CreateDate,
                                          Priority = (j == null) ? "N/A" : j.Priority,
                                          PriorityNo = (j == null) ? "N/A" : j.PriorityNo,
                                          Status = jwr.Status,
                                          Detail = jwr.Detail,
                                          DecisionType = jwr.DecisionType,
                                          SectionType = jwr.SectionType,
                                          Suggestion = jwr.Suggestion,
                                          Requestor = jwr.Requestor,
                                          RequestorName = jwr.RequestorName,
                                          Responsible = jwr.Responsible,
                                          ResponsibleName = jwr.ResponsibleName,
                                          AssignTo = jwr.AssignTo,
                                          AssignToName = jwr.AssignToName
                                      }).ToList();

            query = (from jwr in query.ToList()
                     join wo in _dbITC.WorkOrders.ToList()
                     on jwr.Id equals wo.JobReqBody_Id into joined
                     from j in joined.DefaultIfEmpty()
                     select new JobRequest
                     {
                         Id = jwr.Id,
                         WorkRequest = jwr.WorkRequest,
                         Equipment = jwr.Equipment,
                         Description = jwr.Description,
                         Line = jwr.Line,
                         Symptom = jwr.Symptom,
                         SymptomName_Th = jwr.SymptomName_Th,
                         StandardDate = jwr.StandardDate,
                         CriticalDate = jwr.CriticalDate,
                         CriticalPercent = jwr.CriticalPercent,
                         RequireDate = jwr.RequireDate,
                         CreateDate = jwr.CreateDate,
                         Priority = jwr.Priority,
                         PriorityNo = jwr.PriorityNo,
                         Status = jwr.Status,
                         Detail = jwr.Detail,
                         DecisionType = jwr.DecisionType,
                         SectionType = jwr.SectionType,
                         Suggestion = jwr.Suggestion,
                         Requestor = jwr.Requestor,
                         RequestorName = jwr.RequestorName,
                         Responsible = jwr.Responsible,
                         ResponsibleName = jwr.ResponsibleName,
                         AssignTo = jwr.AssignTo,
                         AssignToName = jwr.AssignToName,
                         WorkOrder_Id = (j == null) ? 0 : j.Id,
                         WoNo = (j == null) ? "" : j.WoNo,
                         StatusWorkOrder = (j == null) ? false : j.Status,
                         Rework = (j == null) ? "0" : (j.Rework.ToString().Length == 1) ? "0" + j.Rework.ToString() : j.Rework.ToString(),
                         Progress = (j == null) ? 0 : j.Progress
                     }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobPlanning()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = (from jwr in ListAssignResponsible().ToList()
                                      join jp in _dbITC.JobPlanning.ToList()
                                      on jwr.WorkOrder_Id equals jp.WorkOrder_Id into joined
                                      from j in joined.DefaultIfEmpty()
                                      select new JobRequest
                                      {
                                          Id = jwr.Id,
                                          WorkRequest = jwr.WorkRequest,
                                          Equipment = jwr.Equipment,
                                          Description = jwr.Description,
                                          Line = jwr.Line,
                                          Symptom = jwr.Symptom,
                                          SymptomName_Th = jwr.SymptomName_Th,
                                          StandardDate = jwr.StandardDate,
                                          CriticalDate = jwr.CriticalDate,
                                          CriticalPercent = jwr.CriticalPercent,
                                          RequireDate = jwr.RequireDate,
                                          CreateDate = jwr.CreateDate,
                                          Priority = jwr.Priority,
                                          PriorityNo = jwr.PriorityNo,
                                          Status = jwr.Status,
                                          Detail = jwr.Detail,
                                          DecisionType = jwr.DecisionType,
                                          SectionType = jwr.SectionType,
                                          Suggestion = (jwr.Suggestion == "") ? "N/A" : jwr.Suggestion,
                                          Requestor = jwr.Requestor,
                                          RequestorName = jwr.RequestorName,
                                          Responsible = jwr.Responsible,
                                          ResponsibleName = jwr.ResponsibleName,
                                          AssignTo = jwr.AssignTo,
                                          AssignToName = jwr.AssignToName,
                                          WorkOrder_Id = jwr.WorkOrder_Id,
                                          WoNo = jwr.WoNo,
                                          StatusWorkOrder = jwr.StatusWorkOrder,
                                          Rework = jwr.Rework,
                                          Progress = jwr.Progress,
                                          PlanStartDate = (j == null) ? "" : String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(j.PlanStartDate)),
                                          PlanFinishDate = (j == null) ? "" : String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(j.PlanFinishDate)),
                                          RootCause = (j == null) ? "" : j.RootCause,
                                          Solution = (j == null) ? "" : j.Solution,
                                          Note = (j == null) ? "" : j.Note
                                      }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobDaily()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = ListJobPlanning().ToList().Join(_dbITC.JobDaily.ToList(),
                jp => jp.WorkOrder_Id,
                jd => jd.WorkOrder_Id,
                (jp, jd) => new JobRequest
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
                    JobDaily_Id = jd.Id,
                    TimeStart = Convert.ToDateTime(jd.TimeStart).ToString("hh:mm tt"),
                    TimeStop = Convert.ToDateTime(jd.TimeStop).ToString("hh:mm tt"),
                    DailyDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(jd.TimeStop))
                }).ToList();

            return query;
        }

        public static List<JobRequest> ListJobDailyTracking()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobRequest> query = (from jp in ListJobPlanning().ToList()
                                      join jd in _dbITC.JobDaily.ToList()
                                      on jp.WorkOrder_Id equals jd.WorkOrder_Id into joined
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
                                          JobDaily_Id = (j == null) ? 0 : j.Id,
                                          TimeStart = (j == null) ? "" : Convert.ToDateTime(j.TimeStart).ToString("hh:mm tt"),
                                          TimeStop = (j == null) ? "" : Convert.ToDateTime(j.TimeStop).ToString("hh:mm tt"),
                                          DailyDate = (j == null) ? "" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(j.TimeStop))
                                      }).ToList();

            return query;
        }

        public static List<CalendarDaily> ListCalendarDaily(string emp_no)
        {
            ITCContext _dbITC = new ITCContext();
            List<CalendarDaily> query = new List<CalendarDaily>();

            if (emp_no == "")
            {
                query = new List<CalendarDaily>();
            }
            else
            {
                query = ListJobPlanning().Where(w => w.AssignTo.EndsWith(emp_no)).ToList().Join(_dbITC.JobDaily.ToList(),
                   jp => jp.WorkOrder_Id,
                   jd => jd.WorkOrder_Id,
                   (jp, jd) => new CalendarDaily
                   {
                       title = "\n" + jp.WoNo + "-" + jp.Rework + "\n" + "DURATION : " + jd.TimeStop.Subtract(jd.TimeStart).ToString().Substring(0, 5),
                       start = jd.TimeStart.ToString("yyyy-MM-ddTHH:mm:ss"),
                       end = jd.TimeStop.ToString("yyyy-MM-ddTHH:mm:ss"),
                       backgroundColor = (jd.Status == 1) ? "#00b5ad" : (jd.Status == 2) ? "#6c757d" : "#28a745",
                       borderColor = (jd.Status == 1) ? "#00b5ad" : (jd.Status == 2) ? "#6c757d" : "#28a745"
                   }).ToList();
            }


            return query;
        }

        public static List<CalendarDaily> ListSchedulePlan(string emp_no)
        {
            ITCContext _dbITC = new ITCContext();
            List<CalendarDaily> ObjList = new List<CalendarDaily>();
            if (emp_no == "")
            {
                ObjList = new List<CalendarDaily>();
            }
            else
            {
                string[] delimiters = new string[] { "," };
                string[] arrEmployeeNo = emp_no.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                string[] arrColor = new string[] { "#cc99ff", "#33ccff", "#0066ff", "#009933", "#cccc00", "#ff9900", "#ff0000", "#cccc00", "#666633", "#009999", "#ff0066" };
                CalendarDaily obj = new CalendarDaily(); 
                for (int i = 0; i < arrEmployeeNo.Length; i++)
                {
                    var item = arrEmployeeNo[i];
                    List<CalendarDaily> query = ListJobPlanning().Where(w => w.AssignTo == item && (w.Status == 5 || w.Status == 6 || w.Status == 7 || w.Status == 8 || w.Status == 9 || w.Status == 12 || w.Status == 13)).ToList()
                        .Select(s => new CalendarDaily
                        {
                            title = "\n" + s.WoNo + "-" + s.Rework + "\n" + "ASSIGN TO : " + s.AssignToName,
                            start = Convert.ToDateTime(s.PlanStartDate).ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = Convert.ToDateTime(s.PlanFinishDate).ToString("yyyy-MM-ddTHH:mm:ss"),
                            backgroundColor = arrColor[i],
                            borderColor = arrColor[i],
                        }).ToList();

                    for (int j = 0; j < query.Count(); j++)
                    {
                        if (query.Count > 0)
                        {
                            obj = new CalendarDaily();
                            obj.title = query[j].title;
                            obj.start = query[j].start;
                            obj.end = query[j].end;
                            obj.backgroundColor = query[j].backgroundColor;
                            obj.borderColor = query[j].borderColor;

                            ObjList.Add(obj);
                        }
                    }
                }
            }

            return ObjList;
        }

        public static List<DailyReport> ListDailyReport()
        {
            ITCContext _dbITC = new ITCContext();
            List<DailyReport> query = _dbITC.WorkOrders.ToList().Join(_dbITC.JobDaily.ToList(),
                wo => wo.Id,
                jd => jd.WorkOrder_Id,
                (wo, jd) => new DailyReport
                {
                    WorkOrder_Id = wo.Id,
                    WoNo = wo.WoNo,
                    DailyDate = String.Format("{0:dd-MMM-yyyy}", jd.DailyDate),
                    Rework = (wo.Rework.ToString().Length == 1) ? "0" + wo.Rework.ToString() : wo.Rework.ToString(),
                    StatusWorkOrderStr = (jd.Status == 1) ? "Doing" : (jd.Status == 2) ? "PM" : "Complete",
                    TimeStart = Convert.ToDateTime(jd.TimeStart).ToString("hh:mm tt"),
                    TimeStop = Convert.ToDateTime(jd.TimeStop).ToString("hh:mm tt")
                }).ToList();

            return query;
        }

        public static List<JobComments> ListJobComments(int id)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobComments> query = _dbITC.JobComment.Where(w => w.JobReqBody_Id == id).ToList().Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                    jc => jc.CreateBy,
                    emp => emp.EMPLOYEE_NO,
                    (jc, emp) => new JobComments
                    {
                        Id = jc.Id,
                        Title = jc.Title,
                        Comment = jc.Comment,
                        CreateBy = jc.CreateBy,
                        CreateDate = (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 60)
                                     ? new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).Seconds + " seconds ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 120)
                                     ? "a minute ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 2700)
                                     ? new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).Minutes + " minutes ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 5400)
                                     ? "an hour ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 86400)
                                     ? new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).Hours + " hours ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 172800)
                                     ? "yesterday"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 2592000)
                                     ? new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).Days + " days ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).TotalSeconds) < 31104000)
                                     ? Convert.ToInt32(Math.Floor((double)new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).Days / 30)) + " months ago"
                                     : Convert.ToInt32(Math.Floor((double)new TimeSpan(DateTime.Now.Ticks - jc.CreateDate.Ticks).Days / 365)) + " years ago",
                        EmployeeName = emp.EMPLOYEE_NAME,
                        ObjJobReply = ListJobRepliesJoinComments(jc.Id, id)
                    }).ToList();

            return query;
        }

        public static List<JobReplies> ListJobReplies()
        {
            ITCContext _dbITC = new ITCContext();
            List<JobReplies> query = _dbITC.JobReply.ToList().Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                    jr => jr.CreateBy,
                    emp => emp.EMPLOYEE_NO,
                    (jr, emp) => new JobReplies
                    {
                        Id = jr.Id,
                        Reply = jr.Reply,
                        CreateBy = jr.CreateBy,
                        CreateDate = (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 60)
                                     ? new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).Seconds + " seconds ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 120)
                                     ? "a minute ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 2700)
                                     ? new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).Minutes + " minutes ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 5400)
                                     ? "an hour ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 86400)
                                     ? new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).Hours + " hours  ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 172800)
                                     ? "yesterday"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 2592000)
                                     ? new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).Days + " days ago"
                                     : (Math.Abs(new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).TotalSeconds) < 31104000)
                                     ? Convert.ToInt32(Math.Floor((double)new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).Days / 30)) + " months ago"
                                     : Convert.ToInt32(Math.Floor((double)new TimeSpan(DateTime.Now.Ticks - jr.CreateDate.Ticks).Days / 365)) + " years ago",
                        EmployeeName = emp.EMPLOYEE_NAME,
                        JobComment_Id = jr.JobComment_Id
                    }).ToList();

            return query;
        }

        public static List<JobReplies> ListJobRepliesJoinComments(int JobComment_Id, int id)
        {
            ITCContext _dbITC = new ITCContext();
            List<JobReplies> query = ListJobReplies().Where(w => w.JobComment_Id == JobComment_Id).ToList().Join(_dbITC.JobComment.Where(w => w.JobReqBody_Id == id).ToList(),
                    jr => jr.JobComment_Id,
                    jc => jc.Id,
                    (jr, jc) => new JobReplies
                    {
                        Id = jr.Id,
                        Reply = jr.Reply,
                        CreateBy = jr.CreateBy,
                        CreateDate = jr.CreateDate,
                        EmployeeName = jr.EmployeeName
                    }).ToList();

            return query;
        }
    }
}