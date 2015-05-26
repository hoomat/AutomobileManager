using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class TransitReportModel
    {

        public string DeliveryDate { get; set; }

        public string ReturnDate { get; set; }

        public int Distance { get; set; }

        public string Automobile { get; set; }

        public string Driver { get; set; }

        public string TrafficCard { get; set; }

        public string Department { get; set; }

    }
}