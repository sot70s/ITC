using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace ITC.Models
{
    [Table("SOP")]
    public class SOP
    {
        [Key]
        public int Id { get; set; }
        public string Procedure { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class SOPS
    {
        public int Id { get; set; }
        public string Procedure { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class ParameterSOP
    {
        public string Procedure { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class SOPInfo
    {
        public static List<SOPS> ListSOP()
        {
            ITCContext _dbITC = new ITCContext();
            List<SOPS> _listSOP = new List<SOPS>();

            _listSOP = _dbITC.SOP.ToList().Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                sop => sop.CreateBy,
                emp => emp.EMPLOYEE_NO,
                (sop, emp) => new SOPS
                {
                    Id = sop.Id,
                    Procedure = sop.Procedure,
                    Type = sop.Type,
                    Description = (sop.Description == null) ? "N/A" : sop.Description,
                    CreateDate = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(sop.CreateDate)),
                    CreateBy = emp.EMPLOYEE_NAME
                }).ToList();

            return _listSOP;
        }
    }

    public class TableSOP
    {
        public List<SOPS> data { get; set; }
    }

    [Table("SOPTemplate")]
    public class SOPTemplate
    {
        [Key]
        public int Id { get; set; }
        public string SOP { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class SOPTemplates
    {
        public int Id { get; set; }
        public string SOP { get; set; }
        public string Type { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class ParameterSOPTemplate
    {
        public string SOP { get; set; }
        public string Type { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
    }


    public class SOPTemplateInfo
    {
        public static List<SOPTemplates> ListSOPTemplate()
        {
            ITCContext _dbITC = new ITCContext();
            List<SOPTemplates> _listSOPTemplate = new List<SOPTemplates>();

            _listSOPTemplate = _dbITC.SOPTemplate.ToList().Join(QueryPersonnel.ListEmployeeMeyer().ToList(),
                sop => sop.CreateBy,
                emp => emp.EMPLOYEE_NO,
                (sop, emp) => new SOPTemplates
                {
                    Id = sop.Id,
                    SOP = sop.SOP,
                    Type = sop.Type,
                    CreateDate = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(sop.CreateDate)),
                    CreateBy = emp.EMPLOYEE_NAME
                }).ToList();

            return _listSOPTemplate;
        }
    }

    public class TableSOPTemplate
    {
        public List<SOPTemplates> data { get; set; }
    }

    [Table("SOPAsset")]
    public class SOPAsset
    {
        [Key]
        public int Id { get; set; }
        public int SOP_Id { get; set; }
        public int SOPTemplate_Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class SOPAssets
    {
        public int Id { get; set; }
        public string Procedure { get; set; }
        public string SOP { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int SOP_Id { get; set; }
        public int SOPTemplate_Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class ParameterSOPAsset
    {
        public int Id { get; set; }
        public int SOPTemplate_Id { get; set; }
        public bool Checkbox { get; set; }
    }

    public class SOPAssetInfo
    {
        public static List<SOPAssets> ListSOPJoinTemplate()
        {
            ITCContext _dbITC = new ITCContext();
            List<SOPAssets> _SOPAssets = _dbITC.SOP.ToList().Join(_dbITC.SOPTemplate.ToList(),
                sop => sop.Type,
                tmp => tmp.Type,
                (sop, tmp) => new SOPAssets
                {
                    Id = sop.Id,
                    Procedure = sop.Procedure,
                    Type = sop.Type,
                    Description = sop.Description,
                    SOPTemplate_Id = tmp.Id
                }).ToList();

            return _SOPAssets;
        }

        public static List<SOPAssets> ListSOPAsset(int id)
        {
            ITCContext _dbITC = new ITCContext();
            List<SOPAssets> _SOPAssets = (from sop in ListSOPJoinTemplate().Where(w => w.SOPTemplate_Id == id).ToList()
                                          join ass in _dbITC.SOPAsset.ToList().Where(w => w.SOPTemplate_Id == id)
                                          on sop.Id equals ass.SOP_Id into joined
                                          from j in joined.DefaultIfEmpty()
                                          select new SOPAssets
                                          {
                                              Id = sop.Id,
                                              Procedure = sop.Procedure,
                                              Type = sop.Type,
                                              Description = (sop.Description == null) ? "N/A" : sop.Description,
                                              SOP_Id = (j == null) ? 0 : j.SOP_Id,
                                              SOPTemplate_Id = (j == null) ? 0 : j.SOPTemplate_Id
                                          }).ToList();

            return _SOPAssets;
        }

    }

    public class TableSOPAssetInfo
    {
        public List<SOPAssets> data { get; set; }
    }
}