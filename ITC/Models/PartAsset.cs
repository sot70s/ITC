using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    [Table("PartAsset")]
    public class PartAsset
    {
        [Key]
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string LotNo { get; set; }
        public string SpareCode { get; set; }
        public string Location { get; set; }
        public string SectionCode { get; set; }
        public string SubSectionCode { get; set; }
        public string InstallDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public int Status { get; set; }
    }
}