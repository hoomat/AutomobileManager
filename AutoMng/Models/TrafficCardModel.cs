using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class TrafficCardModel
    {
        public string PersianDateBuy { get; set; }

        public string PersianDateExpire { get; set; }

        public TrafficCard TrafficCard { get; set; }

        public List<SelectListItem> TrafficCardTypes { get; set; }

        public string TrafficCardType { get; set; }

        public TrafficCardModel()
        {
            PersianDateBuy = PersianDateTime.Now.ToString("yyyy/MM/dd");
            PersianDateExpire = PersianDateTime.Now.ToString("yyyy/MM/dd");
            TrafficCard = new DAL.TrafficCard();
            TrafficCardTypes = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var trafficCardType in db.TrafficCardTypes)
                    TrafficCardTypes.Add(new SelectListItem { Text = trafficCardType.Value.ToString(), Value = trafficCardType.ID.ToString() });
            }
        }

        public TrafficCardModel(TrafficCard trafficCard)
        {
            TrafficCard = trafficCard;
            PersianDateBuy = PersianDateTime.Now.ToString("yyyy/MM/dd");
            PersianDateExpire = PersianDateTime.Now.ToString("yyyy/MM/dd");
            TrafficCardTypes = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var trafficCardType in db.TrafficCardTypes)
                    TrafficCardTypes.Add(new SelectListItem { Text = trafficCardType.Value.ToString(), Value = trafficCardType.ID.ToString() });
            }
        }
    }
}