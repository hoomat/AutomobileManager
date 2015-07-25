using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class AutomobileReportModel
    {
        public string Plaque { get; set; }

        public string Department { get; set; }


        public string Chassis { get; set; }

        public string Model { get; set; }

        public string ProduceYear { get; set; }

        public string Color { get; set; }

        public string FualType { get; set; }

        public string FualConsume { get; set; }

        public string DateBuy { get; set; }

        public int Price { get; set; }

    }
}