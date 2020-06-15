using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    [Table("RoleSR")]
    public class RoleSR
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string SectionType { get; set; }
        public string JobType { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class ParameterRoleSR
    {
        public string EmployeeNo { get; set; }
        public string SectionType { get; set; }
        public string JobType { get; set; }
    }

    public class RoleSRS
    {
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string SectionType { get; set; }
        public string JobType { get; set; }
        public string SectionCode { get; set; }
    }

    public class RoleSRInfo
    {
        public static List<RoleSRS> ListRoleSR()
        {
            ITCContext _dbITC = new ITCContext();
            List<RoleSRS> _listRoleSR = new List<RoleSRS>();

            _listRoleSR = _dbITC.RoleSR.ToList().Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                role => role.EmployeeNo,
                emp => emp.EMPLOYEE_NO,
                (role, emp) => new RoleSRS
                {
                    Id = role.Id,
                    EmployeeNo = role.EmployeeNo,
                    EmployeeName = emp.EMPLOYEE_NAME,
                    SectionType = role.SectionType,
                    JobType = role.JobType,
                    SectionCode = emp.SECTION_CODE
                }).ToList();

            return _listRoleSR;
        }
    }

    public class TableRoleSR
    {
        public List<RoleSRS> data { get; set; }
    }
}