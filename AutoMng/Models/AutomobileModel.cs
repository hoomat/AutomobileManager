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
    public class AutomobileModel
    {
        //public HttpPostedFileBase MyFile { get; set; }
        public Automobile Automobile { get; set; }

        public string PersianDateBuy { get; set; }

        public string PersianLastService { get; set; }

        public MultiSelectList Drivers { get; set; }

        public IEnumerable SelectedDrivers { get; set; }
        public string DriverID { get; set; }

        public List<SelectListItem> Departments { get; set; }

        public string DepartmentID { get; set; }

        public List<SelectListItem> FualTypes { get; set; }

        public AutomobileModel()
        {
            Automobile = new DAL.Automobile();
            PersianDateBuy = PersianDateTime.Now.ToString("yyyy/MM/dd");
            FualTypes = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var fualtype in db.FualTypes)
                    FualTypes.Add(new SelectListItem { Text = fualtype.Value, Value = fualtype.ID.ToString() });
                Drivers = new MultiSelectList(db.Drivers.ToList(), "ID", "Name");
                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }

        public AutomobileModel(Automobile automobile)
        {
            Automobile = automobile;
       
            var persianDateTime = new PersianDateTime(Automobile.DateBuy);
            PersianDateBuy = persianDateTime.ToString("yyyy/MM/dd");
            FualTypes = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var fualtype in db.FualTypes)
                    FualTypes.Add(new SelectListItem { Text = fualtype.Value, Value = fualtype.ID.ToString() });
              //  Drivers = new MultiSelectList(db.Drivers.ToList(), "ID", "Name",(IEnumerable) fg);
                var departments = db.Departments.Where(item => item.ID != automobile.Department.ID).ToList();
                Departments.Add(new SelectListItem { Text = automobile.Department.Name.ToString(), Value = automobile.Department.ID.ToString() });
                foreach (var department in departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() }); 
            }
        }

        public AutomobileModel(bool search)
        {
            
           // var fg = Automobile.AutomobileDrivers.Select(item => item.Driver.ID);
            //var persianDateTime = new PersianDateTime(Automobile.DateBuy);
          //  PersianDateBuy = persianDateTime.ToString("yyyy/MM/dd");
            FualTypes = new List<SelectListItem>();
            FualTypes.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            Departments = new List<SelectListItem>();
            Departments.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var fualtype in db.FualTypes)
                    FualTypes.Add(new SelectListItem { Text = fualtype.Value, Value = fualtype.ID.ToString() });
             //   Drivers = new MultiSelectList(db.Drivers.ToList(), "ID", "Name", (IEnumerable)fg);
                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }
    }
}