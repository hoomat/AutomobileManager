using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class FuelCompareConsumeReportModel
    {
        public string Automobile { get; set; }

        public string AllVolumeFuel { get; set; }

        public string AllAmountPaid { get; set; }

        public string FualConsume { get; set; }

        public string RealFualConsume { get; set; }

        public string Distance { get; set; }
    }
}