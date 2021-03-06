﻿using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AutomobilMng.Models
{
    public class FuelConsumeModel
    {
        public FuelConsume FuelConsume { get; set; }

        public string FualTypeID { get; set; }
        public List<SelectListItem> FualTypes { get; set; }


        public int PaymentTypeID { get; set; }
        public List<SelectListItem> PaymentTypes { get; set; }

        public string AutomobileID { get; set; }
        public List<SelectListItem> Automobiles { get; set; }

        public string FuelCardID { get; set; }
        public List<SelectListItem> FuelCards { get; set; }

        public List<SelectListItem> Drivers { get; set; }

        public string DriverID { get; set; }

        public List<SelectListItem> Departments { get; set; }

        public string DepartmentID { get; set; }

        public string PersianRefuelDate { get; set; }

        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$", ErrorMessage = "ساعت بین 0 تا 23")]
        public string HourRefuelDate { get; set; }

        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$", ErrorMessage = "دقیقه بین 0 تا 59")]
        public string MinuteRefuelDate { get; set; }


        public FuelConsumeModel()
        {

            FuelConsume = new FuelConsume();
            FuelConsume.RefuelDate = DateTime.Now;
            PersianRefuelDate = PersianDateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            Automobiles = new List<SelectListItem>();
            PaymentTypes = new List<SelectListItem>();
            FualTypes = new List<SelectListItem>();
            FuelCards = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            FuelCards.Add(new SelectListItem { Text = "", Value = (-1).ToString() });
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var automobil in db.Automobiles.Where(item => item.AutomobileStatusId == (int)AutomobileStatusModel.Available 
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing).Include(a => a.AutomobileClass))
                    Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });
                foreach(var payment in db.PaymentTypes)
                    PaymentTypes.Add(new SelectListItem { Text = payment.Type.ToString(), Value = payment.ID.ToString() });
                foreach (var fuelCard in db.FuelCards)
                    FuelCards.Add(new SelectListItem { Text = fuelCard.Number.ToString(), Value = fuelCard.ID.ToString() });
                foreach (var fualType in db.FualTypes)
                    FualTypes.Add(new SelectListItem { Text = fualType.Value.ToString(), Value = fualType.ID.ToString() });

                //if (db.Automobiles.FirstOrDefault() != null)
                //    foreach (var automobildriver in db.Automobiles.FirstOrDefault().AutomobileDrivers)
                //        Drivers.Add(new SelectListItem { Text = automobildriver.Driver.Name.ToString(), Value = automobildriver.Driver.ID.ToString() });
                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() }); 
            }
        }

        public FuelConsumeModel(Controller controller)
        {
            FuelConsume = new FuelConsume();
            FuelConsume.RefuelDate = DateTime.Now;
            PersianRefuelDate = PersianDateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            Automobiles = new List<SelectListItem>();
            PaymentTypes = new List<SelectListItem>();
            FuelCards = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            FualTypes = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            FuelCards.Add(new SelectListItem { Text = "", Value = (-1).ToString() });
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobiles.Where(item => item.DepartmentId == user.DepartmentId && 
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
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()) , Value = automobil.ID.ToString() });

                foreach (var payment in db.PaymentTypes)
                    PaymentTypes.Add(new SelectListItem { Text = payment.Type.ToString(), Value = payment.ID.ToString() });
                foreach (var fuelCard in db.FuelCards)
                    FuelCards.Add(new SelectListItem { Text = fuelCard.Number.ToString(), Value = fuelCard.ID.ToString() });
                foreach (var fualType in db.FualTypes)
                    FualTypes.Add(new SelectListItem { Text = fualType.Value.ToString(), Value = fualType.ID.ToString() });

                foreach (var automobildriver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = string.Format("{1} : {0}", automobildriver.Name.ToString(), automobildriver.PersonalNumber.ToString()), Value = automobildriver.ID.ToString() });
                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }

        public FuelConsumeModel(FuelConsume fuelConsume, Controller controller)
        {
            FuelConsume = fuelConsume;
            var persianRefuelDate = new PersianDateTime(FuelConsume.RefuelDate);
            PersianRefuelDate = persianRefuelDate.ToString("yyyy/MM/dd");
            HourRefuelDate = persianRefuelDate.Hour.ToString();
            MinuteRefuelDate = persianRefuelDate.Minute.ToString();
            FualTypes = new List<SelectListItem>();
         //   Automobiles = new List<SelectListItem>();
            PaymentTypes = new List<SelectListItem>();
            FuelCards = new List<SelectListItem>();
        //    Drivers = new List<SelectListItem>();
         //   Departments = new List<SelectListItem>();
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    foreach (var automobil in db.Automobiles)
            //        Automobiles.Add(new SelectListItem { Text =  string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()) + automobil.AutomobileClass.Class.ToString(), Value = automobil.ID.ToString() });
            //    foreach (var payment in db.PaymentTypes)
            //        PaymentTypes.Add(new SelectListItem { Text = payment.Type.ToString(), Value = payment.ID.ToString() });
            //    foreach (var fuelCard in db.FuelCards)
            //        FuelCards.Add(new SelectListItem { Text = fuelCard.Number.ToString(), Value = fuelCard.ID.ToString() });
            //    if (db.Automobiles.FirstOrDefault() != null)
            //        foreach (var automobildriver in db.Automobiles.FirstOrDefault().AutomobileDrivers)
            //            Drivers.Add(new SelectListItem { Text = automobildriver.Driver.Name.ToString(), Value = automobildriver.Driver.ID.ToString() });
            //    foreach (var department in db.Departments)
            //        Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() }); 
            //}

            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
              //  var automobils = db.Automobiles.Where(item => item.ID != fuelConsume.Automobile.ID).ToList();
                Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", fuelConsume.Automobile.Plaque.ToString(), fuelConsume.Automobile.AutomobileClass.Class.ToString()), Value = fuelConsume.Automobile.ID.ToString() });


                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobiles.Where(item => item.ID != fuelConsume.AutomobileID
                        && item.DepartmentId == user.DepartmentId &&
                        (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)).Include(a => a.AutomobileClass))
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobiles.Where(item => item.ID != fuelConsume.AutomobileID &&
                          (item.AutomobileStatusId == (int)AutomobileStatusModel.Available
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Carwash
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Mission
                    || item.AutomobileStatusId == (int)AutomobileStatusModel.Repairing)).Include(a => a.AutomobileClass))
                        Automobiles.Add(new SelectListItem { Text = string.Format("پلاک :{0} - مدل :{1}", automobil.Plaque.ToString(), automobil.AutomobileClass.Class.ToString()), Value = automobil.ID.ToString() });

                var drivers = db.Drivers.Where(item => item.ID != fuelConsume.Driver.ID).ToList();
                Drivers.Add(new SelectListItem { Text = string.Format("{1} : {0}", fuelConsume.Driver.Name.ToString(), fuelConsume.Driver.PersonalNumber.ToString()), Value = fuelConsume.Driver.ID.ToString() });
                foreach (var driver in drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() }); 
                
                
                foreach (var automobildriver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = string.Format("{1} : {0}", automobildriver.Name.ToString(), automobildriver.PersonalNumber.ToString()), Value = automobildriver.ID.ToString() });
                var departments = db.Departments.Where(item => item.ID != fuelConsume.Department.ID).ToList();
                Departments.Add(new SelectListItem { Text = fuelConsume.Department.Name.ToString(), Value = fuelConsume.Department.ID.ToString() });
                foreach (var department in departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
                if (FuelConsume.FuelCard != null)
                {
                    FuelCards.Add(new SelectListItem { Text = FuelConsume.FuelCard.Number.ToString(), Value = FuelConsume.FuelCard.ID.ToString() });
                    foreach (var fuelCard in db.FuelCards.Where(item => item.ID != FuelConsume.FuelCard.ID))
                        FuelCards.Add(new SelectListItem { Text = fuelCard.Number.ToString(), Value = fuelCard.ID.ToString() });
                }
                else
                {
                    FuelCards.Add(new SelectListItem { Text = "", Value = (-1).ToString() });
                    foreach (var fuelCard in db.FuelCards)
                        FuelCards.Add(new SelectListItem { Text = fuelCard.Number.ToString(), Value = fuelCard.ID.ToString() });
                }

                PaymentTypes.Add(new SelectListItem { Text = FuelConsume.PaymentType.Type.ToString(), Value = FuelConsume.PaymentType.ID.ToString() });
                foreach (var payment in db.PaymentTypes.Where(item => item.ID != FuelConsume.PaymentType.ID))
                    PaymentTypes.Add(new SelectListItem { Text = payment.Type.ToString(), Value = payment.ID.ToString() });

                if (FuelConsume.FualType != null)
                    FualTypes.Add(new SelectListItem { Text = FuelConsume.FualType.Value.ToString(), Value = FuelConsume.FualType.ID.ToString() });
                foreach (var fualType in db.FualTypes.Where(item => item.ID != FuelConsume.FualType.ID))
                    FualTypes.Add(new SelectListItem { Text = fualType.Value.ToString(), Value = fualType.ID.ToString() });

            }
        }
    }
}