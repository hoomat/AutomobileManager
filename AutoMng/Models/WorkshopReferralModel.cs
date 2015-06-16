using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{

        public class WorkshopReferralModel
        {
            public int Referral { get; set; }

            public string Workshop { get; set; }
        }

        public class TransitDepartmentDistanceModel
        {
            public int Distance { get; set; }

            public string Department { get; set; }

            public string Automobile { get; set; }
        }

        public class TransitDriverDistanceModel
        {
            public int Distance { get; set; }

            public string Driver { get; set; }
        }
}