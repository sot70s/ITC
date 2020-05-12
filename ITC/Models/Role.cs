using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterRole
    {
        public string Id { get; set; }
        public bool PageStaff { get; set; }
        public bool PagePlanner { get; set; }
        public bool PageUserManager { get; set; }
        public bool PageMisManager { get; set; }
        public int Permission { get; set; }
        public bool Coordinator { get; set; }
    }

    [Table("RolePage")]
    public class RolePage
    {
        [Key]
        public int Id { get; set; }
        public bool PageStaff { get; set; }
        public bool PagePlanner { get; set; }
        public bool PageUserManager { get; set; }
        public bool PageMisManager { get; set; }
        public string MILAuth_Id { get; set; }
    }

    [Table("RoleUser")]
    public class RoleUser
    {
        [Key]
        public int Id { get; set; }
        public bool Permission { get; set; }
        public string MILAuth_Id { get; set; }
    }

    [Table("RoleCoordinator")]
    public class RoleCoordinator
    {
        [Key]
        public int Id { get; set; }
        public bool Coordinator { get; set; }
        public string MILAuth_Id { get; set; }
    }

    public class RoleAll
    {
        public bool PageStaff { get; set; }
        public bool PagePlanner { get; set; }
        public bool PageUserManager { get; set; }
        public bool PageMisManager { get; set; }
        public bool Permission { get; set; }
        public bool Coordinator { get; set; }
        public string MILAuth_Id { get; set; }
    }

    public class QueryRole
    {
        public static List<RoleAll> ListRole()
        {
            ITCContext _dbITC = new ITCContext();
            List<RoleAll> query = _dbITC.RolePage.ToList().Join(_dbITC.RoleUser.ToList(),
                    rp => rp.MILAuth_Id,
                    ru => ru.MILAuth_Id,
                    (rp, ru) => new RoleAll
                    {
                        MILAuth_Id = rp.MILAuth_Id,
                        PageStaff = rp.PageStaff,
                        PagePlanner = rp.PagePlanner,
                        PageUserManager = rp.PageUserManager,
                        PageMisManager = rp.PageMisManager,
                        Permission = ru.Permission
                    }).Join(_dbITC.RoleCoordinator.ToList(),
                    rpu => rpu.MILAuth_Id,
                    rc => rc.MILAuth_Id,
                    (rpu,rc) => new RoleAll
                    {
                        MILAuth_Id = rpu.MILAuth_Id,
                        PageStaff = rpu.PageStaff,
                        PagePlanner = rpu.PagePlanner,
                        PageUserManager = rpu.PageUserManager,
                        PageMisManager = rpu.PageMisManager,
                        Permission = rpu.Permission,
                        Coordinator = rc.Coordinator
                    }).ToList();

            return query;
        }
    }
}