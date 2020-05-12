using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    public class ParameterApprover
    {
        public string EmployeeNo { get; set; }
        public string SectionCode { get; set; }
    }

    public class ParameterRejectJob
    {
        public int Id { get; set; }
        public string Reason { get; set; }
    }

    [Table("Approver")]
    public class Approver
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string SectionCode { get; set; }
    }

    [Table("RejectJobApprover")]
    public class RejectJobApprover
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
        public int JobReqBody_Id { get; set; }
        public string RejectBy { get; set; }
    }

    [Table("JobApprove")]
    public class JobApprove
    {
        [Key]
        public int Id { get; set; }
        public int JobReqBody_Id { get; set; }
        public string ApproveBy { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ApproverJoinAccount
    {
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string Email { get; set; }
        public string SectionCode { get; set; }
    }

    public class TableApproverJoinAccount
    {
        public List<ApproverJoinAccount> data { get; set; }
    }

    public class QueryApprover
    {
        public static List<ApproverJoinAccount> ListApprover()
        {
            ITCContext _dbITC = new ITCContext();
            List<ApproverJoinAccount> query = QueryAccount.ListAccountMeyer().Join(_dbITC.Approver.ToList(),
                                                                 acc => acc.EmployeeNo,
                                                                 apv => apv.EmployeeNo,
                                                                 (acc, apv) => new ApproverJoinAccount
                                                                 {
                                                                     Id = apv.Id,
                                                                     EmployeeNo = apv.EmployeeNo,
                                                                     DEPARTMENT_CODE = acc.DEPARTMENT_CODE,
                                                                     DEPARTMENT_DESCRIPTION = acc.DEPARTMENT_DESCRIPTION,
                                                                     SECTION_CODE = acc.SECTION_CODE,
                                                                     SECTION_DESCRIPTION = acc.SECTION_DESCRIPTION,
                                                                     EMPLOYEE_NAME = acc.EMPLOYEE_NAME,
                                                                     Email = acc.Email,
                                                                     POSITION_DESCRIPTION = acc.POSITION_DESCRIPTION,
                                                                     SectionCode = apv.SectionCode
                                                                 }).ToList();
            return query;
        }
    }
}