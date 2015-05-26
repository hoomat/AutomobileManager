using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class RepairModel
    {
        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$", ErrorMessage = "ساعت بین 0 تا 23")]
        public string RepairHour { get; set; }

        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$", ErrorMessage = "دقیقه بین 0 تا 59")]
        public string RepairMinute { get; set; }

        public string PersianDateRepair { get; set; }

        public Repair Repair { get; set; }

        public List<ConsumablePartModel> ConsumablePartModels { get; set; }
        public string AutomobileID { get; set; }
        public List<SelectListItem> Automobiles { get; set; }

        public string DriverID { get; set; }
        public List<SelectListItem> Drivers { get; set; }

        public List<SelectListItem> Departments { get; set; }

        public string DepartmentID { get; set; }

        public RepairModel(Controller controller, int incidentid)
        {
            Repair = new DAL.Repair();
            Repair.IncidentID = incidentid;
            PersianDateRepair = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            ConsumablePartModels = new List<ConsumablePartModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.DirectorGeneral)
                    foreach (var automobil in db.Automobils)
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                foreach (var automobildriver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = automobildriver.Name.ToString(), Value = automobildriver.ID.ToString() });
            }
        }

        public RepairModel(Controller controller)
        {
            Repair = new DAL.Repair();
            PersianDateRepair = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            ConsumablePartModels = new List<ConsumablePartModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.DirectorGeneral)
                    foreach (var automobil in db.Automobils)
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                foreach (var automobildriver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = automobildriver.Name.ToString(), Value = automobildriver.ID.ToString() });
            }
        }

        public RepairModel()
        {
           // ConsumablePartModels = new List<ConsumablePartModel>();
            Repair = new DAL.Repair();
            PersianDateRepair = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                foreach (var automobil in db.Automobils)
                    Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                foreach (var automobildriver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = automobildriver.Name.ToString(), Value = automobildriver.ID.ToString() });
                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
            }
        }

        public RepairModel(Repair repair, Controller controller)
        {
            Repair = repair;
            var persianDateTime = new PersianDateTime(Repair.DateRepair);
            PersianDateRepair = persianDateTime.ToString("yyyy/MM/dd");
            ConsumablePartModels = new List<ConsumablePartModel>();
            RepairHour = persianDateTime.Hour.ToString();
            RepairMinute = persianDateTime.Minute.ToString();
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var consumeable in db.ConsumableParts.Where(item => item.RepairId == repair.ID))
                    ConsumablePartModels.Add(new ConsumablePartModel
                    {
                        Id = consumeable.Id,
                        Name = consumeable.Name,
                        Price = consumeable.Price,
                        Type = consumeable.Type
                    });
                        
             
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                var automobils = db.Automobils.Where(item => item.ID != repair.Automobile.ID).ToList();
                Automobiles.Add(new SelectListItem { Text = repair.Automobile.Plaque.ToString(), Value = repair.Automobile.ID.ToString() });

                if (user.GroupId == (int)GroupModel.DirectorGeneral)
                    foreach (var automobil in db.Automobils)
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                var drivers = db.Drivers.Where(item => item.ID != repair.Driver.ID).ToList();
                Drivers.Add(new SelectListItem { Text = repair.Driver.Name.ToString(), Value = repair.Driver.ID.ToString() });
                foreach (var driver in drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                if (repair.Department != null)
                {
                    var departments = db.Departments.Where(item => item.ID != repair.Department.ID).ToList();
                    Departments.Add(new SelectListItem { Text = repair.Department.Name.ToString(), Value = repair.Department.ID.ToString() });
                    foreach (var department in departments)
                        Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
                }
                else
                {
                    foreach (var department in db.Departments)
                        Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
                }

            }
        }

        public bool Define(IPrincipal user)
        {
            try
            {
                Repair.Cost = Repair.Wage;
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    Repair.ConsumableParts = new List<ConsumablePart>();
                    if (ConsumablePartModels != null)
                        foreach (var consumablePart in ConsumablePartModels)
                        {
                            Repair.ConsumableParts.Add(new ConsumablePart { Name = consumablePart.Name, Price = consumablePart.Price, Type = consumablePart.Type });
                            Repair.Cost = Repair.Cost + consumablePart.Price;
                        }
                    var persianDateTime = PersianDateTime.Parse(PersianDateRepair + " " + RepairHour + ":" + RepairMinute + ":00");
                    Repair.DateRepair = persianDateTime.ToDateTime();
                    Repair.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == user.Identity.Name);
                    var driverid = int.Parse(DriverID);
                    Repair.DriverID = driverid;
                    var automobileid = int.Parse(AutomobileID);
                    Repair.AutomobileID = automobileid;
                    var departmentid = int.Parse(DepartmentID);
                    Repair.DepartmentId = departmentid;
                    applicationDbContext.Repairs.Add(Repair);
                    applicationDbContext.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Modify()
        {
            try
            {
               
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    var repair = applicationDbContext.Repairs.FirstOrDefault(item => item.ID == Repair.ID);
                    repair.Cost = Repair.Wage;
                    if (repair.ConsumableParts == null)
                        repair.ConsumableParts = new List<ConsumablePart>();
                    if (ConsumablePartModels == null)
                    {
                        var consumableParts = applicationDbContext.ConsumableParts.Where(item => item.RepairId == repair.ID).ToList();
                        foreach (var consumablePart in consumableParts)
                        {
                           
                            applicationDbContext.ConsumableParts.Remove(consumablePart);
                        }
                    }
                    else
                    {
                        var consumableParts = applicationDbContext.ConsumableParts.Where(item => item.RepairId == repair.ID).ToList();
                        var notexist = consumableParts.Where(item => !ConsumablePartModels.Any(n => n.Id == item.Id));
                        foreach (var consumablePart in notexist)
                        { applicationDbContext.ConsumableParts.Remove(consumablePart); }
                        foreach (var consumablePart in ConsumablePartModels)
                        {
                            var searchconsumablePart = applicationDbContext.ConsumableParts.FirstOrDefault(item => item.Id == consumablePart.Id);
                            if (searchconsumablePart != null)
                            {
                                searchconsumablePart.Name = consumablePart.Name;
                                searchconsumablePart.Price = consumablePart.Price;
                                searchconsumablePart.Type = consumablePart.Type;
                                repair.Cost = repair.Cost + consumablePart.Price;
                            }
                            else
                            {
                                repair.ConsumableParts.Add(new ConsumablePart { Name = consumablePart.Name, Price = consumablePart.Price, Type = consumablePart.Type, RepairId = repair.ID });
                                repair.Cost = repair.Cost + consumablePart.Price;
                            }
                          
                        }
                    }
                    var persianDateTime = PersianDateTime.Parse(PersianDateRepair + " " + RepairHour + ":" + RepairMinute + ":00");
                    repair.DateRepair = persianDateTime.ToDateTime();
                    var driverid = int.Parse(DriverID);
                    repair.DriverID = driverid;
                    var automobileid = int.Parse(AutomobileID);
                    repair.AutomobileID = automobileid;
                    repair.Wage = Repair.Wage;
                    var departmentid = int.Parse(DepartmentID);
                    repair.DepartmentId = departmentid;
                    //applicationDbContext.Entry(Repair).State = System.Data.Entity.EntityState.Modified;
                    applicationDbContext.SaveChanges();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    var repair = applicationDbContext.Repairs.First(u => u.ID == Repair.ID);
                    applicationDbContext.Repairs.Remove(repair);
                    applicationDbContext.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}