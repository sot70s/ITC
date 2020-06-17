using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public string ItemNo { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Property { get; set; }
        public string SerailNumber { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public int Status { get; set; }

    }

    [Table("InventoryType")]
    public class InventoryType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }

    }

    public class ParameterInventory
    {
        public int Id { get; set; }
        public string ItemNo { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Property { get; set; }
        public string SerailNumber { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public int Status { get; set; }
    }

    public class TableInventory
    {
        public List<Inventory> data { get; set; }
    }

    public class QueryInventory
    {
        public static List<Inventory> ListItemAsset()
        {
            ITCContext _dbITC = new ITCContext();
            List<Inventory> query = _dbITC.Inventory.ToList();

            return query;
        }
    }
}