using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class TransitDistanceModel
    {
        public int Distance { get; set; }

        public string Automobile { get; set; }
    }

    public class RepairCostModel
    {
        public int Cost { get; set; }

        public string Automobile { get; set; }
    }

    public class FuelConsumeChartModel
    {
        public int Cost { get; set; }

        public int Volume { get; set; }

        public string Automobile { get; set; }
    }
}