using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class IncidentSearchModel
    {
        public string FromPersianIncidentDate { get; set; }
        public string ToPersianIncidentDate { get; set; }
    

        public List<SelectListItem> Automobiles { get; set; }

        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }
        public string DriverID { get; set; }


        public IncidentSearchModel(Controller controller)
        {
            Automobiles = new List<SelectListItem>();
            Automobiles.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            Drivers = new List<SelectListItem>();
            Drivers.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var identityUser = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                if (identityUser.Group.Id == 1)
                      foreach (var automobil in db.Automobils)
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == identityUser.DepartmentId))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

            }
        }
    }
}