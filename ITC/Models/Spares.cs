using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    [Table("SparesCode")]
    public class SparesCode
    {
        [Key]
        public int Id { get; set; }
        public string SpareCode { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    [Table("SparesCodeTemplate")]
    public class SparesCodeTemplate
    {
        [Key]
        public int Id { get; set; }
        public string SpareCode { get; set; }
        public string ItemNo { get; set; }
    }

    public class ParameterSpares
    {
        public int Id { get; set; }
        public string SpareCode { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class TableSpares
    {
        public List<SparesCode> data { get; set; }
    }

    public class QuerySparePart
    {
        public static List<SparesCode> ListSparesPart()
        {
            ITCContext _dbITC = new ITCContext();
            List<SparesCode> query = _dbITC.SparesCode.ToList();

            return query;
        }
    }

}