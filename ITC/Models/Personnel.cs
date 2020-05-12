using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    [Table("HRM_Employee")]
    public class HRM_Employee
    {
        [Key]
        public string EMPLOYEE_NO { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_LOCAL_NAME { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_DESCRIPTION { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public string GRADE_CODE { get; set; }
        public string SEX { get; set; }
    }
    [Table("HRM_Employee_Manager")]
    public class HRM_Employee_Manager
    {
        [Key]
        public string EMPLOYEE_NO { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_LOCAL_NAME { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_DESCRIPTION { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public string GRADE_CODE { get; set; }
        public string SEX { get; set; }
    }
    [Table("HRM_Section_Master")]
    public class HRM_Section_Master
    {
        [Key]
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_DESCRIPTION { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
    }

    public class EmployeeStore
    {
        public string EMPLOYEE_NO { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_LOCAL_NAME { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_DESCRIPTION { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public string GRADE_CODE { get; set; }
        public string SEX { get; set; }
    }
    public class TableHRM_Employee
    {
        public List<HRM_Employee> data { get; set; }
    }
    public class TableHRM_Employee_Manager
    {
        public List<HRM_Employee_Manager> data { get; set; }
    }
    public class TableSectionJoinGroupCoordinator
    {
        public List<SectionJoinGroupCoordinator> data { get; set; }
    }

    public class EmployeeJoinSoftware
    {
        public string EMPLOYEE_NO { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string SECTION_CODE { get; set; }
        public string Equip_Id { get; set; }
        public string Equipment { get; set; }
        public string SoftwareName { get; set; }
        public string CoreFunction { get; set; }
        public string Position { get; set; }
    }

    public class QueryPersonnel
    {
        public static List<EmployeeStore> ListEmployee()
        {
            HRMContext _dbHRM = new HRMContext();
            List<EmployeeStore> queryHrmEmployee = _dbHRM.HRM_Employee
                .Select(s => new EmployeeStore
                {
                    EMPLOYEE_NO = s.EMPLOYEE_NO,
                    EMPLOYEE_NAME = s.EMPLOYEE_NAME,
                    EMPLOYEE_LOCAL_NAME = s.EMPLOYEE_LOCAL_NAME,
                    EMPLOYEE_STATUS = s.EMPLOYEE_STATUS,
                    DIVISION_CODE = s.DIVISION_CODE,
                    DIVISION_DESCRIPTION = s.DIVISION_DESCRIPTION,
                    DEPARTMENT_CODE = s.DEPARTMENT_CODE,
                    DEPARTMENT_DESCRIPTION = s.DEPARTMENT_DESCRIPTION,
                    SECTION_CODE = s.SECTION_CODE,
                    SECTION_DESCRIPTION = s.SECTION_DESCRIPTION,
                    POSITION_DESCRIPTION = s.POSITION_DESCRIPTION,
                    GRADE_CODE = s.GRADE_CODE,
                    SEX = s.SEX
                }).ToList();

            return queryHrmEmployee;
        }

        public static List<EmployeeStore> ListEmployeeManager()
        {
            HRMContext _dbHRM = new HRMContext();
            List<EmployeeStore> queryHrmEmployeeManager = _dbHRM.HRM_Employee_Manager
                .Select(s => new EmployeeStore
                {
                    EMPLOYEE_NO = s.EMPLOYEE_NO,
                    EMPLOYEE_NAME = s.EMPLOYEE_NAME,
                    EMPLOYEE_LOCAL_NAME = s.EMPLOYEE_LOCAL_NAME,
                    EMPLOYEE_STATUS = s.EMPLOYEE_STATUS,
                    DIVISION_CODE = s.DIVISION_CODE,
                    DIVISION_DESCRIPTION = s.DIVISION_DESCRIPTION,
                    DEPARTMENT_CODE = s.DEPARTMENT_CODE,
                    DEPARTMENT_DESCRIPTION = s.DEPARTMENT_DESCRIPTION,
                    SECTION_CODE = s.SECTION_CODE,
                    SECTION_DESCRIPTION = s.SECTION_DESCRIPTION,
                    POSITION_DESCRIPTION = s.POSITION_DESCRIPTION,
                    GRADE_CODE = s.GRADE_CODE,
                    SEX = s.SEX
                }).ToList();

            return queryHrmEmployeeManager;
        }

        public static List<EmployeeStore> ListEmployeeMeyer()
        {
            List<EmployeeStore> query = ListEmployee().ToList().Union(ListEmployeeManager().ToList()).ToList();

            return query;
        }
    }
}