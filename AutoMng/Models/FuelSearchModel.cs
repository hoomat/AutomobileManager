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
    public class FuelSearchModel
    {
        public string FualStation { get; set; }

        public List<SelectListItem> Automobiles { get; set; }

        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }
        public string DriverID { get; set; }

        public FuelSearchModel(Controller controller)
        {
            Automobiles = new List<SelectListItem>();
            Automobiles.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            Drivers = new List<SelectListItem>();
            Drivers.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var identityUser = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobiles.Where(item => item.DepartmentId == identityUser.DepartmentId &&
                          (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)).Include(a => a.AutomobileClass))
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobiles.Where(item => (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)).Include(a => a.AutomobileClass))
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = string.Format("{1} : {0}", driver.Name.ToString(), driver.PersonalNumber.ToString()), Value = driver.ID.ToString() });


            }
        }
    }
}