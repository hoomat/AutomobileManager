using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AutomobilMng.Models
{
    public class TransitSearchModel
    {
        public string FromPersianDeliveryDate { get; set; }
        public string ToPersianDeliveryDate { get; set; }
        public string FromPersianReturnDate { get; set; }
        public string ToPersianReturnDate { get; set; }

        public List<SelectListItem> Automobiles { get; set; }

        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }
        public string DriverID { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public string DepartmentID { get; set; }
        
        public List<SelectListItem> CardTraffics { get; set; }
        public string CardTrafficID { get; set; }

        public TransitSearchModel(Controller controller)
        {
            Automobiles = new List<SelectListItem>();
            Automobiles.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            Drivers = new List<SelectListItem>();
            Drivers.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            Departments = new List<SelectListItem>();
            Departments.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            CardTraffics = new List<SelectListItem>();
            CardTraffics.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobiles.Where(item => item.DepartmentId == user.DepartmentId ).Include(a => a.AutomobileClass))
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });
                  
                else
                    foreach (var automobil in db.Automobiles.Include(a => a.AutomobileClass))
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = string.Format("{1} : {0}", driver.Name.ToString(), driver.PersonalNumber.ToString()), Value = driver.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                foreach (var trafficCard in db.TrafficCards.Where(item => item.DateExpire >= DateTime.Now))
                    CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
            }
        }
    }
}