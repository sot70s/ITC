using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    [Table("Lot")]
    public class Lot
    {
        [Key]
        public int Id { get; set; }
        public string LotNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string StartWarrantyDate { get; set; }
        public string ExpireWarrantyDate { get; set; }
        public string ServiceVendor { get; set; }
        public string ContactVendor { get; set; }
        public int Quantity { get; set; }
        public string WareHouse { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ItemNo { get; set; }
    }

    public class ParameterLot
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string StartWarrantyDate { get; set; }
        public string ExpireWarrantyDate { get; set; }
        public string ServiceVendor { get; set; }
        public string ContactVendor { get; set; }
        public int Quantity { get; set; }
        public string WareHouse { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ItemNo { get; set; }
    }

    public class TableLot
    {
        public List<Lot> data { get; set; }
    }

    public class QueryLot
    {
        public static List<Lot> ListItemAsset()
        {
            ITCContext _dbITC = new ITCContext();
            List<Lot> query = _dbITC.Lot.ToList();

            return query;
        }
    }
}