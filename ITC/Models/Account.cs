using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterAccount
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    [Table("Accounts")]
    public class Accounts
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string EmployeeNo { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class AccountJoinEmployee
    {
        [Key]
        public string Id { get; set; }
        public string EmployeeNo { get; set; }
        public string UserName { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string Email { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public string CreatedDate { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public List<RoleAll> Role { get; set; }
        public bool Permission { get; set; }
        public bool PageStaff { get; set; }
        public bool PagePlanner { get; set; }
        public bool PageUserManager { get; set; }
        public bool PageMisManager { get; set; }
    }

    public class TableAccountsJoinEmployee
    {
        public List<AccountJoinEmployee> data { get; set; }
    }

    public class QueryAccount
    {
        public static List<AccountJoinEmployee> ListEmployee()
        {
            MILAuthContext _dbMILAuth = new MILAuthContext();
            HRMContext _dbHRM = new HRMContext();
            List<AccountJoinEmployee> query = _dbMILAuth.Accounts.ToList().Join(_dbHRM.HRM_Employee.ToList(),
                    acc => acc.EmployeeNo,
                    hrm => hrm.EMPLOYEE_NO,
                    (acc, hrm) => new AccountJoinEmployee
                    {
                        Id = acc.Id,
                        EmployeeNo = acc.EmployeeNo,
                        UserName = acc.UserName,
                        EMPLOYEE_NAME = hrm.EMPLOYEE_NAME,
                        Email = acc.Email,
                        DIVISION_CODE = hrm.DIVISION_CODE,
                        DEPARTMENT_CODE = hrm.DEPARTMENT_CODE,
                        DEPARTMENT_DESCRIPTION = hrm.DEPARTMENT_DESCRIPTION,
                        SECTION_CODE = hrm.SECTION_CODE,
                        SECTION_DESCRIPTION = hrm.SECTION_DESCRIPTION,
                        POSITION_DESCRIPTION = hrm.POSITION_DESCRIPTION,
                        CreatedDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", acc.CreatedDate),
                        EMPLOYEE_STATUS = hrm.EMPLOYEE_STATUS
                    }).ToList();

            return query;
        }

        public static List<AccountJoinEmployee> ListManager()
        {
            MILAuthContext _dbMILAuth = new MILAuthContext();
            HRMContext _dbHRM = new HRMContext();
            List<AccountJoinEmployee> query = _dbMILAuth.Accounts.ToList().Join(_dbHRM.HRM_Employee_Manager.ToList(),
            acc => acc.EmployeeNo,
            hrm => hrm.EMPLOYEE_NO,
            (acc, hrm) => new AccountJoinEmployee
            {
                Id = acc.Id,
                EmployeeNo = acc.EmployeeNo,
                UserName = acc.UserName,
                EMPLOYEE_NAME = hrm.EMPLOYEE_NAME,
                Email = acc.Email,
                DIVISION_CODE = hrm.DIVISION_CODE,
                DEPARTMENT_CODE = hrm.DEPARTMENT_CODE,
                DEPARTMENT_DESCRIPTION = hrm.DEPARTMENT_DESCRIPTION,
                SECTION_CODE = hrm.SECTION_CODE,
                SECTION_DESCRIPTION = hrm.SECTION_DESCRIPTION,
                POSITION_DESCRIPTION = hrm.POSITION_DESCRIPTION,
                CreatedDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", acc.CreatedDate),
                EMPLOYEE_STATUS = hrm.EMPLOYEE_STATUS
            }).ToList();

            return query;
        }

        public static List<AccountJoinEmployee> ListAccountMeyer()
        {
            List<AccountJoinEmployee> query = ListEmployee().Union(ListManager()).Distinct().GroupBy(g => new {
                g.Id,
                g.EmployeeNo,
                g.UserName,
                g.EMPLOYEE_NAME,
                g.Email,
                g.DIVISION_CODE,
                g.DEPARTMENT_CODE,
                g.DEPARTMENT_DESCRIPTION,
                g.SECTION_CODE,
                g.SECTION_DESCRIPTION,
                g.POSITION_DESCRIPTION,
                g.CreatedDate,
                g.EMPLOYEE_STATUS
            }, (key, group) => new AccountJoinEmployee
            {
                Id = key.Id,
                EmployeeNo = key.EmployeeNo,
                UserName = key.UserName,
                EMPLOYEE_NAME = key.EMPLOYEE_NAME,
                Email = key.Email,
                DIVISION_CODE = key.DIVISION_CODE,
                DEPARTMENT_CODE = key.DEPARTMENT_CODE,
                DEPARTMENT_DESCRIPTION = key.DEPARTMENT_DESCRIPTION,
                SECTION_CODE = key.SECTION_CODE,
                SECTION_DESCRIPTION = key.SECTION_DESCRIPTION,
                POSITION_DESCRIPTION = key.POSITION_DESCRIPTION,
                CreatedDate = key.CreatedDate,
                EMPLOYEE_STATUS = key.EMPLOYEE_STATUS
            }).ToList();

            return query;
        }

        public static List<AccountJoinEmployee> ListRole()
        {
            List<AccountJoinEmployee> query = ListAccountMeyer().GroupJoin(QueryRole.ListRole(),
            acc => acc.Id,
            role => role.MILAuth_Id,
            (acc, role) => new AccountJoinEmployee
            {
                Id = acc.Id,
                EmployeeNo = acc.EmployeeNo,
                UserName = acc.UserName,
                EMPLOYEE_NAME = acc.EMPLOYEE_NAME,
                Email = acc.Email,
                SECTION_CODE = acc.SECTION_CODE,
                CreatedDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", acc.CreatedDate),
                Role = role.ToList()
            }).ToList();

            return query;
        }

        public static List<AccountJoinEmployee> ListRoleCoordinatorOnly()
        {
            ITCContext _dbITC = new ITCContext();
            List<AccountJoinEmployee> query = ListAccountMeyer().Join(_dbITC.RoleCoordinator.Where(w =>w.Coordinator == true),
            acc => acc.Id,
            role => role.MILAuth_Id,
            (acc, role) => new AccountJoinEmployee
            {
                Id = acc.Id,
                EmployeeNo = acc.EmployeeNo,
                UserName = acc.UserName,
                EMPLOYEE_NAME = acc.EMPLOYEE_NAME,
                DEPARTMENT_CODE = acc.DEPARTMENT_CODE,
                Email = acc.Email,
                SECTION_CODE = acc.SECTION_CODE,
                CreatedDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", acc.CreatedDate),
            }).ToList();

            return query;
        }

        public static List<AccountJoinEmployee> ListAllRole()
        {
            List<AccountJoinEmployee> query = ListAccountMeyer().Join(QueryRole.ListRole(),
            acc => acc.Id,
            role => role.MILAuth_Id,
            (acc, role) => new AccountJoinEmployee
            {
                Id = acc.Id,
                EmployeeNo = acc.EmployeeNo,
                UserName = acc.UserName,
                EMPLOYEE_NAME = acc.EMPLOYEE_NAME,
                Email = acc.Email,
                SECTION_CODE = acc.SECTION_CODE,
                CreatedDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", acc.CreatedDate),
                Permission = role.Permission,
                PageStaff = role.PageStaff,
                PagePlanner = role.PagePlanner,
                PageUserManager = role.PageUserManager,
                PageMisManager = role.PageMisManager
            }).ToList();

            return query;
        }
    }
}