using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    public class StatusChart
    {
        public string Status { get; set; }
        public string Color { get; set; }
        public int CountStatus { get; set; }
    }

    public class JobStatusOnHandChart
    {
        public string AssignToName { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public int CountStatus { get; set; }
        public int Reworked { get; set; }
        public int Proceed { get; set; }
        public int Complete { get; set; }
    }
}