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
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ServiceDate { get; set; }
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



}


