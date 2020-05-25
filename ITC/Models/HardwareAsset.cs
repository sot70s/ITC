using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    public class ParameterHardwareAsset
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
        public string SafetyNote { get; set; }
    }

    public class Parameter
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string ContactVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public string PurchaseDate { get; set; }
        public string ReceiveDate { get; set; }
        public string ServiceDate { get; set; }
        public string WarrantyDate { get; set; }
        public string EquipmentAging { get; set; }
        public string LastCalibateDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
        public string SafetyNote { get; set; }

    }



    public class ParameterPersonal
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string ContactVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public string PurchaseDate { get; set; }
        public string ReceiveDate { get; set; }
        public string ServiceDate { get; set; }
        public string WarrantyDate { get; set; }
        public string EquipmentAging { get; set; }
        public string LastCalibateDate { get; set; }
        public string SafetyNote { get; set; }

    }

    [Table("HardwareAsset")]
    public class HardwareAsset
    {
        [Key]
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
        public string SafetyNote { get; set; }

    }

    public class TableHardwareAsset
    {

        public List<HardwareAsset> data { get; set; }

    }

    public class QueryHardwareAsset
    {
        public static List<HardwareAsset> ListHardwareAsset()
        {
            ITCContext dblist = new ITCContext();
            List<HardwareAsset> query = dblist.HardwareAssets.ToList();

            return query;
        }
    }
    public class QueryHardwareAssetList
    {
        public static List<Parameter> HardwareAssetList()
        {
            ITCContext dblist = new ITCContext();
            List<Parameter> query = dblist.HardwareAssets.ToList()
                .Select(s => new Parameter
                {
                    Id = s.Id,
                    Equipment = (s.Equipment == null) ? "N/A" : s.Equipment,
                    EquipmentType = (s.EquipmentType == null) ? "N/A" : s.EquipmentType,
                    Description = (s.Description == null) ? "N/A" : s.Description,
                    SerialNumber = (s.SerialNumber == null) ? "N/A" : s.SerialNumber,
                    Model = (s.Model == null) ? "N/A" : s.Model,
                    Owner = (s.Owner == null) ? "N/A" : s.Owner,
                    Location = (s.Location == null) ? "N/A" : s.Location,
                    TelephoneOwner = (s.TelephoneOwner == null) ? "N/A" : s.TelephoneOwner,
                    Responsible = (s.Responsible == null) ? "N/A" : s.Responsible,
                    Status = s.Status,
                    PreviousOwner = (s.PreviousOwner == null) ? "N/A" : s.PreviousOwner,
                    PreviousLocation = (s.PreviousLocation == null) ? "N/A" : s.PreviousLocation,
                    ServiceVendor = (s.ServiceVendor == "Select") ? "N/A" : s.ServiceVendor,
                    Company = (s.Company == "Select") ? "N/A" : s.Company,
                    EquipmentGroup = (s.EquipmentGroup == "Select") ? "N/A" : s.EquipmentGroup,
                    PurchaseDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)),
                    ReceiveDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)),
                    ServiceDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)),
                    WarrantyType = (s.WarrantyType == "Select") ? "N/A" : s.WarrantyType,
                    WarrantyNum = s.WarrantyNum,
                    SafetyNote = (s.SafetyNote == null) ? "N/A" : s.SafetyNote,
                }).ToList();
            return query;
        }

        public static List<ParameterPersonal> PersonalList(int id)
        {
            ITCContext _dbITC = new ITCContext();
            var strdd = "";
            var strmm = "";
            var stryy = "";
            var st = "";
            List<ParameterPersonal> query3 = (from EQT in _dbITC.HardwareAssets.Where(w => w.Id == id).ToList()
                                              join eq in QueryPersonnel.ListEmployeeMeyer().ToList()
                                              on EQT.Owner equals eq.EMPLOYEE_NO
                                              select new ParameterPersonal
                                              {
                                                  Id = EQT.Id,
                                                  Equipment = EQT.Equipment,
                                                  EquipmentType = (EQT.EquipmentType == null) ? "N/A" : EQT.EquipmentType,
                                                  Description = (EQT.Description == null) ? "N/A" : EQT.Description,
                                                  SerialNumber = (EQT.SerialNumber == null) ? "N/A" : EQT.SerialNumber,
                                                  Model = (EQT.Model == null) ? "N/A" : EQT.Model,
                                                  Owner = (EQT.Owner == null) ? "N/A" : EQT.Owner,
                                                  Location = (EQT.Location == null) ? "N/A" : EQT.Location,
                                                  Section = (eq.SECTION_CODE == null) ? "N/A" : eq.SECTION_CODE,
                                                  Department = (eq.DEPARTMENT_CODE == null) ? "N/A" : eq.DEPARTMENT_CODE,
                                                  TelephoneOwner = (EQT.TelephoneOwner == null) ? "N/A" : EQT.TelephoneOwner,
                                                  PreviousOwner = (EQT.PreviousOwner == null) ? "N/A" : EQT.PreviousOwner,
                                                  PreviousLocation = (EQT.PreviousLocation == null) ? "N/A" : EQT.PreviousLocation,
                                                  Responsible = (EQT.Responsible == null) ? "N/A" : EQT.Responsible,
                                                  ServiceVendor = (EQT.ServiceVendor == null) ? "N/A" : EQT.ServiceVendor,
                                                  //ContactVendor = (EQT.ContactVendor == null) ? "N/A" : EQT.ContactVendor,
                                                  Company = (EQT.Company == "Select") ? "N/A" : EQT.Company,
                                                  EquipmentGroup = (EQT.EquipmentGroup == "Select") ? "N/A" : EQT.EquipmentGroup,
                                                  PurchaseDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)),
                                                  ReceiveDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)),
                                                  ServiceDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)),
                                                  //WarrantyDate = ,
                                                  //LastCalibateDate = (EQT.LastCalibateDate == null) ? "N/A" : EQT.LastCalibateDate,
                                                  SafetyNote = (EQT.SafetyNote == null) ? "N/A" : EQT.SafetyNote,
                                                  EquipmentAging = (Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) >= 365 ?
                                                                       strdd = (Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) / 365).ToString() + " Year " +
                                                                               ((Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) % 365) / 30).ToString() + "Month " +
                                                                               (((Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) % 365)) % 30).ToString() + " Day"
                                                                    : Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) >= 30 ?
                                                                       strdd = (Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) / 30).ToString() + " Month " +
                                                                                (Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) % 30).ToString() + " Day"
                                                                    : Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) < 30 ?
                                                                       strdd = Convert.ToInt32((Convert.ToDateTime(EQT.PurchaseDate) - DateTime.Now).Days.ToString().Replace("-", "")) + " Day"
                                                                    : "N/A"),

                                              }).ToList();

            return query3;
        }
    }

    public class QueryEquipmentLast
    {
        public static List<HardwareAsset> ListEquipmentLast()
        {
            ITCContext dblist = new ITCContext();
            List<HardwareAsset> query = dblist.HardwareAssets.ToList()
                .Select(s => new HardwareAsset
                {
                    Id = s.Id,
                    Equipment = s.Equipment,
                    EquipmentType = s.EquipmentType,

                }).ToList();

            return query;
        }
    }



    //public static List<ParameterPersonal> HardwareList()
    //{
    //    ITCContext dblist = new ITCContext();
    //    List<ParameterPersonal> query = dblist.HardwareAssets.ToList()
    //        .Select(s => new ParameterPersonal
    //        {
    //            Id = s.Id,
    //            Equipment = (s.Equipment == null) ? "N/A" : s.Equipment,
    //            EquipmentType = (s.EquipmentType == null) ? "N/A" : s.EquipmentType,
    //            Description = (s.Description == null) ? "N/A" : s.Description,
    //            SerialNumber = (s.SerialNumber == null) ? "N/A" : s.SerialNumber,
    //            Model = (s.Model == null) ? "N/A" : s.Model,
    //            Owner = (s.Owner == null) ? "N/A" : s.Owner,
    //            Location = (s.Location == null) ? "N/A" : s.Location,
    //            TelephoneOwner = (s.TelephoneOwner == null) ? "N/A" : s.TelephoneOwner,
    //            Responsible = (s.Responsible == null) ? "N/A" : s.Responsible,
    //            PreviousOwner = (s.PreviousOwner == null) ? "N/A" : s.PreviousOwner,
    //            PreviousLocation = (s.PreviousLocation == null) ? "N/A" : s.PreviousLocation,
    //            ServiceVendor = (s.ServiceVendor == "Select") ? "N/A" : s.ServiceVendor,
    //            Company = (s.Company == "Select") ? "N/A" : s.Company,
    //            EquipmentGroup = (s.EquipmentGroup == "Select") ? "N/A" : s.EquipmentGroup,
    //            PurchaseDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)),
    //            ReceiveDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)),
    //            ServiceDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)),
    //            SafetyNote = (s.SafetyNote == null) ? "N/A" : s.SafetyNote,
    //        }).ToList();
    //    return query;
    //}



    //public static List<ParameterPersonal> PersonalList()
    //{
    //    List<ParameterPersonal> query = HardwareList().ToList().Union(ListPersonal().ToList()).ToList();

    //    return query;
    //}
}


