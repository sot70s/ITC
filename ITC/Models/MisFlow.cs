using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterMisFlow
    {
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string Division { get; set; }
        public string JobType { get; set; }
    }


    [Table("MisFlow")]
    public class MisFlow
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string Division { get; set; }
        public string JobType { get; set; }
    }

    public class MisFlowJoinEmployee
    {
        public int Id { get; set; }
        public string Division { get; set; }
        public string JobType { get; set; }
        public string EmployeeNo { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string Email { get; set; }
    }

    public class TableMisFlowJoinEmployee
    {
        public List<MisFlowJoinEmployee> data { get; set; }
    }

    public class QueryMisFlow
    {
        public static List<MisFlowJoinEmployee> ListMisFlow()
        {
            ITCContext _dbITC = new ITCContext();

            List<MisFlowJoinEmployee> query = _dbITC.MisFlow.ToList().Join(QueryAccount.ListAccountMeyer().Where(w => w.EMPLOYEE_STATUS == "A").ToList(),
                                                                mf => mf.EmployeeNo,
                                                                emp => emp.EmployeeNo,
                                                                (mf, emp) => new MisFlowJoinEmployee
                                                                {
                                                                    Id = mf.Id,
                                                                    EmployeeNo = emp.EmployeeNo,
                                                                    EMPLOYEE_NAME = emp.EMPLOYEE_NAME,
                                                                    Division = mf.Division,
                                                                    JobType = mf.JobType,
                                                                    Email = emp.Email
                                                                }).ToList();

            return query;
        }
    }
}