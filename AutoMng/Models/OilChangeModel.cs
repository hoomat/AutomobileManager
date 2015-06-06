using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class OilChangeModel
    {
        public string PersianChangeDate { get; set; }

        public string OilFilterChanged { get; set; }
        public string AirFilterChanged { get; set; }

        public OilChange OilChange { get; set; }

        public List<SelectListItem> Automobiles { get; set; }

        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }

        public string DriverID { get; set; }

        public List<SelectListItem> Departments { get; set; }

        public string DepartmentID { get; set; }

        public OilChangeModel()
        { }
        public OilChangeModel(Controller controller)
        {
            OilChange = new DAL.OilChange();
            PersianChangeDate = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobiles.Where(item => item.DepartmentId == user.DepartmentId && 
                          (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobiles.Where(item => (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }

        public OilChangeModel(OilChange oilChange, Controller controller)
        {
            if (oilChange.AirFilterChanged)
                AirFilterChanged = "on";
            if (oilChange.OilFilterChanged)
                OilFilterChanged = "on";
            OilChange = oilChange;
            var persianDateTime = new PersianDateTime(oilChange.ChangeDate);
            PersianChangeDate = persianDateTime.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Automobiles.Add(new SelectListItem { Text = oilChange.Automobile.Plaque.ToString(), Value = oilChange.Automobile.ID.ToString() });
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobiles.Where(item => item.DepartmentId == user.DepartmentId && item.ID != oilChange.AutomobileID &&
                          (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobiles.Where(item => item.ID != oilChange.AutomobileID && 
                        (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });


                var drivers = db.Drivers.Where(item => item.ID != oilChange.Driver.ID).ToList();
                Drivers.Add(new SelectListItem { Text = oilChange.Driver.Name.ToString(), Value = oilChange.Driver.ID.ToString() });
                foreach (var driver in drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });
                var departments = db.Departments.Where(item => item.ID != oilChange.Department.ID).ToList();
                Departments.Add(new SelectListItem { Text = oilChange.Department.Name.ToString(), Value = oilChange.Department.ID.ToString() });
                foreach (var department in departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }
    }
}