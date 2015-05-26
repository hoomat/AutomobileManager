using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class RepiarReportModel
    {
        public string Commander { get; set; }

        public string DateRepair { get; set; }

        public string Workshop { get; set; }

        public string Cost { get; set; }

        public string Automobile  { get; set; }
        
        public  string Driver { get; set; }
    }
}