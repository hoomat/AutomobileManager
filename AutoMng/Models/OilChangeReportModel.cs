using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class OilChangeReportModel
    {

        public string CaseChange { get; set; }
        public string DateChange { get; set; }

        public string Workshop { get; set; }

        public string OilType { get; set; }

        public string Automobile { get; set; }

        public string Driver { get; set; }
    }
}