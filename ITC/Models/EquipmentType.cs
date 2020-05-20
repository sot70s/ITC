using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{

    public class ParameterEquipmentType
    {
        public int Id { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
    public class ParameterEQ
    {
        public int Id { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }


    [Table("EquipmentType")]
    public class Equipment_Type
    {
        [Key]
        public int Id { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }

    public class TableEquipmentType
    {
        public List<Equipment_Type> data { get; set; }
    }

    public class QueryEquipmentType
    {
        public static List<Equipment_Type> ListEquipmentType()
        {

            ITCContext dblist = new ITCContext();
            List<Equipment_Type> query = dblist.Equipment_Types.ToList();

            return query;

        }
    }

    public class QueryListEquipmentType
    {
        public static List<Equipment_Type> listEquipmentType()
        {
            ITCContext dblist = new ITCContext();
            List<Equipment_Type> query = dblist.Equipment_Types
               .Select(s => new Equipment_Type
               {
                   Id = s.Id,
                   EquipmentType = s.EquipmentType,
                   Description = s.Description,
                   Status = s.Status,


               }).ToList();

            return query;
        }

    }

}






