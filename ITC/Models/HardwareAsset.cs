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
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public string PurchaseDate { get; set; }
        public string ReceiveDate { get; set; }
        public string ServiceDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
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
                    Equipment = s.Equipment,
                    EquipmentType = s.EquipmentType,
                    Description = s.Description,
                    SerialNumber = s.SerialNumber,
                    Model = s.Model,
                    Owner = s.Owner,
                    Location = s.Location,
                    TelephoneOwner = s.TelephoneOwner,
                    Responsible = s.Responsible,
                    Status = s.Status,
                    PreviousOwner = s.PreviousOwner,
                    PreviousLocation = s.PreviousLocation,
                    ServiceVendor = s.ServiceVendor,
                    Company = s.Company,
                    EquipmentGroup = s.EquipmentGroup,
                    PurchaseDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)),
                    ReceiveDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)),
                    ServiceDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)),
                    WarrantyType = s.WarrantyType,
                    WarrantyNum = s.WarrantyNum,
                    SafetyNote = s.SafetyNote,

                }).ToList();

            return query;
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




}


