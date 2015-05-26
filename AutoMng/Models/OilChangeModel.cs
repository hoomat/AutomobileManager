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
        {
            OilChange = new DAL.OilChange();
            PersianChangeDate = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var automobil in db.Automobils)
                    Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                //if (db.Automobils.FirstOrDefault() != null)
                //    foreach (var automobildriver in db.Automobils.FirstOrDefault().AutomobileDrivers)
                //        Drivers.Add(new SelectListItem { Text = automobildriver.Driver.Name.ToString(), Value = automobildriver.Driver.ID.ToString() });
                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }

        public OilChangeModel(OilChange oilChange)
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
                var automobils = db.Automobils.Where(item => item.ID != oilChange.Automobile.ID).ToList();
                Automobiles.Add(new SelectListItem { Text = oilChange.Automobile.Plaque.ToString(), Value = oilChange.Automobile.ID.ToString() });
                foreach (var automobil in automobils)
                    Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                //foreach (var automobildriver in oilChange.Automobile.AutomobileDrivers)
                //    Drivers.Add(new SelectListItem { Text = automobildriver.Driver.Name.ToString(), Value = automobildriver.Driver.ID.ToString() });
                var departments = db.Departments.Where(item => item.ID != oilChange.Department.ID).ToList();
                Departments.Add(new SelectListItem { Text = oilChange.Department.Name.ToString(), Value = oilChange.Department.ID.ToString() });
                foreach (var department in departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }
    }
}