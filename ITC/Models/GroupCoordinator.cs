using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterGroupCoordinator
    {
        public string SectionCode { get; set; }
        public string EmployeeNo { get; set; }
        public bool Checkbox { get; set; }
    }

    [Table("GroupCoordinator")]
    public class GroupCoordinator
    {
        [Key]
        public int Id { get; set; }
        public string SectionCode { get; set; }
        public string EmployeeNo { get; set; }
    }

    public class SectionJoinGroupCoordinator
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string EmployeeNo { get; set; }
    }

    public class QueryGroupCoordinator
    {
        public static List<SectionJoinGroupCoordinator> ListSection(string employee_no)
        {
            ITCContext _dbITC = new ITCContext();
            HRMContext _dbHRM = new HRMContext();

            List<SectionJoinGroupCoordinator> query = (from hsm in _dbHRM.HRM_Section_Master.ToList()
                                                                                  join gc in _dbITC.GroupCoordinator.Where(w => w.EmployeeNo == employee_no).ToList()
                                                                                  on hsm.SECTION_CODE equals gc.SectionCode into joined
                                                                                  from j in joined.DefaultIfEmpty()
                                                                                  select new SectionJoinGroupCoordinator
                                                                                  {
                                                                                      DEPARTMENT_CODE = hsm.DEPARTMENT_CODE,
                                                                                      DEPARTMENT_DESCRIPTION = hsm.DEPARTMENT_DESCRIPTION,
                                                                                      SECTION_CODE = hsm.SECTION_CODE,
                                                                                      SECTION_DESCRIPTION = hsm.SECTION_DESCRIPTION,
                                                                                      EmployeeNo = (j == null) ? "" : j.EmployeeNo
                                                                                  }).OrderBy(o => o.DEPARTMENT_CODE).ThenBy(t => t.SECTION_CODE).ToList();
            return query;
        }
    }
}