using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace ITC.Models
{
    [Table("SystemRequestHeader")]
    public class SystemRequestHeader
    {
        [Key]
        public int Id { get; set; }
        public string SystemRequest { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public bool Status { get; set; }
    }

    public class ParameterDurationh
    {
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
    }
}