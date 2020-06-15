using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITC.Models
{
    public class ParameterSoftware
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string SoftwareType { get; set; }
        public string SoftwareName { get; set; }
        public string Responsible { get; set; }
        public string SupportBy { get; set; }
        public string CoreFunction { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string Location { get; set; }
        public int Cost { get; set; }
        public string LicenseType { get; set; }
        public int LicenseQty { get; set; }
        public string StartDate { get; set; }
        public string ObsolateDate { get; set; }
        public bool UserAccountNeed { get; set; }
        public bool TrainingNeed { get; set; }
        public bool Status { get; set; }
        public string Position { get; set; }
    }

    public class ParameterSoftwareHolder
    {
        public string EmployeeNo { get; set; }
        public string Equipment { get; set; }
        public bool Checkbox { get; set; }
    }

    public class ParameterSoftwareTemplate
    {
        public string Position { get; set; }
        public string Equipment { get; set; }
        public bool Checkbox { get; set; }
    }

    [Table("SoftwareAsset")]
    public class Software
    {
        [Key]
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string SoftwareType { get; set; }
        public string SoftwareName { get; set; }
        public string Responsible { get; set; }
        public string Company { get; set; }
        public string CoreFunction { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string Location { get; set; }
        public int Cost { get; set; }
        public string LicenseType { get; set; }
        public int LicenseQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ObsolateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool UserAccountNeed { get; set; }
        public bool TrainingNeed { get; set; }
        public bool Status { get; set; }
        public string SupportBy { get; set; }
    }

    [Table("SoftwareFile")]
    public class SoftwareFile
    {
        [Key]
        public int Id { get; set; }
        public string SoftwareFilePath { get; set; }
        public string Equip_Id { get; set; }
    }

    [Table("SoftwareHolder")]
    public class SoftwareHolder
    {
        [Key]
        public int Id { get; set; }
        public string Equip_Id { get; set; }
        public string EmployeeNo { get; set; }
    }

    [Table("SoftwareTemplate")]
    public class SoftwareTemplate
    {
        [Key]
        public int Id { get; set; }
        public string Equip_Id { get; set; }
        public string Position { get; set; }
    }

    [Table("EQUIP")]
    public class EQUIP
    {
        [Key]
        public string EQNUM { get; set; }
        public string DESCRIPTION { get; set; }
        public string UD9 { get; set; }
        public string PERSONRESPONSIBLE { get; set; }
        public string INSERVICE { get; set; }
        public string EQTYPE { get; set; }
        public string SUBLOCATION1 { get; set; }
        public string SUBLOCATION2 { get; set; }
    }

    [Table("EQTYPE")]
    public class EQUIPTYPE
    {
        [Key]
        public string EQTYPE { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public class SoftwarareStore
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string SoftwareName { get; set; }
        public string Company { get; set; }
        public string Responsible { get; set; }
        public string SupportBy { get; set; }
        public string CoreFunction { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string StartDate { get; set; }
        public string ObsolateDate { get; set; }
        public bool Status { get; set; }
        public string Equip_Id { get; set; }
        public string EmployeeNo { get; set; }
        public string SoftwareType { get; set; }
        public string Location { get; set; }
        public int Cost { get; set; }
        public string LicenseType { get; set; }
        public int LicenseQty { get; set; }
        public bool UserAccountNeed { get; set; }
        public bool TrainingNeed { get; set; }
        public string SoftwareFilePath { get; set; }
    }

    public class HardwareStore
    {
        public string EQNUM { get; set; }
        public string DESCRIPTION { get; set; }
        public string OWNER { get; set; }
        public string RESPONSIBLE { get; set; }
        public string EQTYPE { get; set; }
        public string EQTYPE_DESC { get; set; }
    }

    public class AnotherStore
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public string EmployeeNo { get; set; }
        public bool Coordinator { get; set; }
        public string SectionCode { get; set; }
        public string Owner { get; set; }
    }

    public class EquipmentStore
    {
        public string Equipment { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Responsible { get; set; }
    }

    public class TableSoftware
    {
        public List<SoftwarareStore> data { get; set; }
    }

    public class TableSoftwareHolder
    {
        public List<EmployeeJoinSoftware> data { get; set; }
    }

    public class QuerySoftware
    {
        public static List<SoftwarareStore> ListSoftware()
        {
            ITCContext _dbITC = new ITCContext();
            List<SoftwarareStore> query = _dbITC.Software
                .Select(s => new SoftwarareStore
                {
                    Id = s.Id,
                    Equipment = s.Equipment,
                    SoftwareName = s.SoftwareName,
                    Company = s.Company,
                    CoreFunction = s.CoreFunction,
                    Description = s.Description,
                    Remark = s.Remark,
                    Responsible = s.Responsible,
                    SupportBy = s.SupportBy,
                    StartDate = s.StartDate.ToString(),
                    ObsolateDate = s.ObsolateDate.ToString(),
                    Status = s.Status,
                    Location = s.Location,
                    Cost = s.Cost,
                    LicenseType = s.LicenseType,
                    LicenseQty = s.LicenseQty,
                    UserAccountNeed = s.UserAccountNeed,
                    TrainingNeed = s.TrainingNeed
                }).ToList()
                .Select(s => new SoftwarareStore
                {
                    Id = s.Id,
                    Equipment = s.Equipment,
                    SoftwareName = s.SoftwareName,
                    Company = s.Company,
                    CoreFunction = s.CoreFunction,
                    Description = s.Description,
                    Remark = s.Remark,
                    Responsible = s.Responsible,
                    SupportBy = s.SupportBy,
                    StartDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", Convert.ToDateTime(s.StartDate)),
                    ObsolateDate = String.Format("{0:dd-MMM-yyyy HH:mm tt}", Convert.ToDateTime(s.ObsolateDate)),
                    Status = s.Status,
                    Location = s.Location,
                    Cost = s.Cost,
                    LicenseType = s.LicenseType,
                    LicenseQty = s.LicenseQty,
                    UserAccountNeed = s.UserAccountNeed,
                    TrainingNeed = s.TrainingNeed
                }).ToList();

            query = (from sw in query.ToList()
                     join sf in _dbITC.SoftwareFile.ToList()
                     on sw.Equipment equals sf.Equip_Id into joined
                     from j in joined.DefaultIfEmpty()
                     select new SoftwarareStore
                     {
                         Id = sw.Id,
                         Equipment = sw.Equipment,
                         SoftwareName = sw.SoftwareName,
                         Company = sw.Company,
                         CoreFunction = sw.CoreFunction,
                         Description = sw.Description,
                         Remark = sw.Remark,
                         Responsible = sw.Responsible,
                         SupportBy = sw.SupportBy,
                         StartDate = sw.StartDate,
                         ObsolateDate = sw.ObsolateDate,
                         Status = sw.Status,
                         Location = sw.Location,
                         Cost = sw.Cost,
                         LicenseType = sw.LicenseType,
                         LicenseQty = sw.LicenseQty,
                         UserAccountNeed = sw.UserAccountNeed,
                         TrainingNeed = sw.TrainingNeed,
                         SoftwareFilePath = (j == null) ? "" : j.SoftwareFilePath,
                     }).ToList();

            return query;
        }

        public static List<EmployeeJoinSoftware> ListSoftwareHolder(string emp_no)
        {
            ITCContext _dbITC = new ITCContext();
            List<EmployeeJoinSoftware> query = (from sw in _dbITC.Software.ToList()
                                                join sh in _dbITC.SoftwareHolder.Where(w => w.EmployeeNo == emp_no)
                                                on sw.Equipment equals sh.Equip_Id into joined
                                                from j in joined.DefaultIfEmpty()
                                                select new EmployeeJoinSoftware
                                                {
                                                    SoftwareName = sw.SoftwareName,
                                                    CoreFunction = (sw.CoreFunction == "") ? "N/A" : sw.CoreFunction,
                                                    Equipment = sw.Equipment,
                                                    EMPLOYEE_NO = (j == null) ? "" : j.EmployeeNo,
                                                    Equip_Id = (j == null) ? "" : j.Equip_Id,
                                                }).OrderBy(o => o.Equip_Id).ToList();
            return query;
        }

        public static List<EmployeeJoinSoftware> ListSoftwareTemplate(string position)
        {
            ITCContext _dbITC = new ITCContext();
            List<EmployeeJoinSoftware> query = (from sw in _dbITC.Software.ToList()
                                                join st in _dbITC.SoftwareTemplate.Where(w => w.Position == position)
                                                on sw.Equipment equals st.Equip_Id into joined
                                                from j in joined.DefaultIfEmpty()
                                                select new EmployeeJoinSoftware
                                                {
                                                    SoftwareName = sw.SoftwareName,
                                                    CoreFunction = (sw.CoreFunction == "") ? "N/A" : sw.CoreFunction,
                                                    Equipment = sw.Equipment,
                                                    Position = (j == null) ? "" : j.Position,
                                                    Equip_Id = (j == null) ? "" : j.Equip_Id,
                                                }).OrderBy(o => o.Equip_Id).ToList();
            return query;
        }

        public static List<SoftwarareStore> ListSoftwareOnHand()
        {
            ITCContext _dbITC = new ITCContext();
            List<SoftwarareStore> query = _dbITC.Software.ToList().Join(_dbITC.SoftwareHolder.ToList(),
                                        sw => sw.Equipment,
                                        swh => swh.Equip_Id,
                                        (sw, swh) => new SoftwarareStore
                                        {
                                            Equipment = sw.Equipment,
                                            SoftwareName = sw.SoftwareName,
                                            EmployeeNo = swh.EmployeeNo,
                                            Responsible = sw.Responsible
                                        }).ToList();
            return query;
        }
    }

    public class QueryHardware
    {
        public static List<HardwareStore> ListHardware()
        {
            MP2Context _dbMP2 = new MP2Context();
            List<HardwareStore> query = (from eq in _dbMP2.EQUIP.Where(w => w.EQNUM.StartsWith("0E") && w.INSERVICE == "Y" && w.EQTYPE != "0ETEL" && w.EQTYPE != "0EAU" && w.EQTYPE != null).ToList()
                                         join eqt in _dbMP2.EQTYPE
                                         on eq.EQTYPE equals eqt.EQTYPE into joined
                                         from j in joined.DefaultIfEmpty()
                                         select new HardwareStore
                                         {
                                             EQNUM = eq.EQNUM,
                                             DESCRIPTION = eq.DESCRIPTION,
                                             OWNER = (eq.UD9 == null) ? "" : eq.UD9,
                                             RESPONSIBLE = (eq.PERSONRESPONSIBLE == null) ? "" : eq.PERSONRESPONSIBLE.Substring(0, eq.PERSONRESPONSIBLE.Length - 2),
                                             EQTYPE = eq.EQTYPE,
                                             EQTYPE_DESC = (j == null) ? "" : j.DESCRIPTION
                                         }).ToList();
            return query;
        }
    }


    public class QueryAnother
    {
        public static List<AnotherStore> ListHardware()
        {
            MP2Context _dbMP2 = new MP2Context();
            List<AnotherStore> query = QueryHardware.ListHardware()
                .Select(s => new AnotherStore
                {
                    Equipment = s.EQNUM,
                    Description = s.DESCRIPTION,
                    Owner = s.OWNER
                }).ToList();
            //.Join(QueryPersonnel.ListEmployeeMeyer(),
            //hw => hw.Owner,
            //emp => emp.EMPLOYEE_NO,
            //(hw, emp) => new AnotherStore
            //{
            //    Equipment = hw.Equipment,
            //    Description = hw.Description,
            //    Owner = emp.EMPLOYEE_NO,
            //    SectionCode = emp.SECTION_CODE
            //}).ToList();

            query = (from hw in query.ToList()
                     join emp in QueryPersonnel.ListEmployeeMeyer().ToList()
                     on hw.Owner equals emp.EMPLOYEE_NO into joined
                     from j in joined.DefaultIfEmpty()
                     select new AnotherStore
                     {
                         Equipment = hw.Equipment,
                         Description = hw.Description,
                         Owner = (j == null) ? "" : j.EMPLOYEE_NO,
                         SectionCode = (j == null) ? "" : j.SECTION_CODE
                     }).ToList();

            return query;
        }

        public static List<AnotherStore> ListSoftware()
        {
            List<AnotherStore> query = QuerySoftware.ListSoftwareOnHand().ToList()
                .Select(s => new AnotherStore
                {
                    Equipment = s.Equipment,
                    Description = s.SoftwareName,
                    Owner = s.EmployeeNo
                }).Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                hw => hw.Owner,
                emp => emp.EMPLOYEE_NO,
                (sw, emp) => new AnotherStore
                {
                    Equipment = sw.Equipment,
                    Description = sw.Description,
                    Owner = emp.EMPLOYEE_NO,
                    SectionCode = emp.SECTION_CODE
                }).ToList();

            return query;
        }

        public static List<AnotherStore> ListAnother()
        {
            List<AnotherStore> query = ListHardware().ToList().Union(ListSoftware().ToList()).ToList();
            return query;
        }

        public static List<AnotherStore> ListAnotherOnHand(string emp_no)
        {
            MILAuthContext _dbMILAuth = new MILAuthContext();
            ITCContext _dbITC = new ITCContext();

            List<AnotherStore> query = _dbMILAuth.Accounts.ToList().Where(w => w.EmployeeNo == emp_no).Join(_dbITC.RoleCoordinator.ToList(),
                    acc => acc.Id,
                    rc => rc.MILAuth_Id,
                    (acc, rc) => new AnotherStore
                    {
                        EmployeeNo = acc.EmployeeNo,
                        Coordinator = rc.Coordinator
                    }).Where(w => w.Coordinator == true).Join(_dbITC.GroupCoordinator.ToList(),
                    acc => acc.EmployeeNo,
                    gc => gc.EmployeeNo,
                    (acc, gc) => new AnotherStore
                    {
                        EmployeeNo = acc.EmployeeNo,
                        SectionCode = gc.SectionCode
                    }).Join(ListAnother().ToList(),
                    acc => acc.SectionCode,
                    sec => sec.SectionCode,
                    (acc, eq) => new AnotherStore
                    {
                        Equipment = eq.Equipment,
                        Description = eq.Description,
                        SectionCode = acc.SectionCode,
                        Owner = eq.Owner
                    }).ToList();

            return query;
        }
    }

    public class QueryEquipment
    {
        public static List<EquipmentStore> ListEquipment()
        {
            MP2Context _dbMP2 = new MP2Context();
            List<EquipmentStore> querySoftware = QuerySoftware.ListSoftwareOnHand()
                .Select(s => new EquipmentStore
                {
                    Equipment = s.Equipment,
                    Description = s.SoftwareName,
                    Owner = s.EmployeeNo,
                    Responsible = s.Responsible
                }).ToList();

            List<EquipmentStore> queryHardware = _dbMP2.EQUIP
                            .Select(s => new EquipmentStore
                            {
                                Equipment = s.EQNUM,
                                Description = s.DESCRIPTION,
                                Owner = s.UD9,
                                Responsible = (s.PERSONRESPONSIBLE == null) ? "" : s.PERSONRESPONSIBLE.Substring(0, s.PERSONRESPONSIBLE.Length - 2),
                            }).ToList();

            List<EquipmentStore> query = querySoftware.Union(queryHardware).ToList();

            return query;
        }
    }
}