using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class FuelConsumeReportModel
    {
        public string RefuelDate { get; set; }

        public int VolumeFuel { get; set; }

        public int AmountPaid { get; set; }

        public string FualStation { get; set; }

        public string  PaymentType { get; set; }

        public string Automobile { get; set; }

        public string Driver  { get; set; }

        public string Department  { get; set; }

    }
}