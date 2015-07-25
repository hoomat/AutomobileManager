using DAL;
using MD.PersianDateTime;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class AutomobileSearchModel
    {
        public string Plaque { get; set; }

        public string Chassis { get; set; }

        public string Model { get; set; }

        public string ProduceYear { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public string DepartmentId { get; set; }


        public List<SelectListItem> FualTypes { get; set; }
        public string FualTypeId { get; set; }

        public List<SelectListItem> TrafficCardTypes { get; set; }
        public string TrafficCardType { get; set; }

        public List<SelectListItem> CardTraffics { get; set; }
        public string CardTrafficID { get; set; }

        public AutomobileSearchModel()
        {
            FualTypes = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            TrafficCardTypes = new List<SelectListItem>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var fualtype in db.FualTypes)
                    FualTypes.Add(new SelectListItem { Text = fualtype.Value, Value = fualtype.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                foreach (var trafficCards in db.TrafficCards)
                    TrafficCardTypes.Add(new SelectListItem { Text = trafficCards.NumberCard, Value = trafficCards.ID.ToString() });

                foreach (var trafficCardType in db.TrafficCardTypes)
                    TrafficCardTypes.Add(new SelectListItem { Text = trafficCardType.Value, Value = trafficCardType.ID.ToString() });
            }
        }
    }
}