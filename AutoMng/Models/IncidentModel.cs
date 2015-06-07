using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class IncidentModel
    {

        [Required(ErrorMessage = "زمان تصادف الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$",ErrorMessage = "ساعت بین 0 تا 23" )]
        public string HourIncident { get; set; }


        [Required(ErrorMessage = "زمان تصادف الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$",ErrorMessage = "دقیقه بین 0 تا 59")  ]
        public string MinuteIncident { get; set; }

        public string PersianIncidentDate { get; set; }

        public Incident Incident { get; set; }

        public List<Damage> Damages { get; set; }

      //  public string Damage { get; set; }

        public List<SelectListItem> Automobiles { get; set; }
        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }
        public string DriverID { get; set; }

        public IncidentModel(Controller controller)
        {
            Incident = new DAL.Incident();
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils)
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                    foreach (var driver in db.Drivers)
                        Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });
            }
        }

        public IncidentModel()
        {
            PersianIncidentDate = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Incident = new DAL.Incident();
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var automobil in db.Automobils)
                    Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });
            }
        }

        public IncidentModel(Incident incident, Controller controller)
        {
            Incident = incident;
            var deliverydatetime = new PersianDateTime(incident.IncidentDate);
            PersianIncidentDate = deliverydatetime.ToString("yyyy/MM/dd HH:mm:ss");

            HourIncident = deliverydatetime.Hour.ToString();
            MinuteIncident = deliverydatetime.Minute.ToString();
            Damages = new List<Damage>();
            if (Incident.Damages != null && Incident.Damages.Any())
            {
                foreach (var damage in Incident.Damages)
                {
                    Damages.Add(damage);
                }
            }
            Automobiles = new List<SelectListItem>();
            Drivers= new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var automobils = db.Automobils.Where(item => item.ID != incident.Automobile.ID).ToList();
                Automobiles.Add(new SelectListItem { Text = incident.Automobile.Plaque.ToString(), Value = incident.Automobile.ID.ToString() });
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils)
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                var drivers = db.Drivers.Where(item => item.ID != incident.Driver.ID).ToList();
                Drivers.Add(new SelectListItem { Text = incident.Driver.Name.ToString(), Value = incident.Driver.ID.ToString() });
                foreach (var driver in drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });
            }
        }

        public bool Define(IPrincipal principal)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var persianDeliveryDate = PersianDateTime.Parse(PersianIncidentDate + " " + HourIncident + ":" + MinuteIncident + ":00");
                    Incident.IncidentDate = persianDeliveryDate.ToDateTime();


                    Incident.IdentityUser = db.Users.FirstOrDefault(item => item.UserName == principal.Identity.Name);

                    Incident.Damages = new List<Damage>();
                 
                        foreach (var damage in Damages)
                            Incident.Damages.Add(damage);

                    var automobileID = int.Parse(AutomobileID);
                    Incident.AutomobileID = automobileID;



                    var driverid = int.Parse(DriverID);
                    Incident.DriverID = driverid;


                    db.Incidents.Add(Incident);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public  bool Delete()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var incident = db.Incidents.First(u => u.ID == Incident.ID);
                    db.Incidents.Remove(incident);
                    db.SaveChanges();
                }
                return true;
            }
            catch { return false; }
        }


        public  bool Modify()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var persianDeliveryDate = PersianDateTime.Parse(PersianIncidentDate + " " + HourIncident + ":" + HourIncident + ":00");
                    Incident.IncidentDate = persianDeliveryDate.ToDateTime();


                    var incident = db.Incidents.FirstOrDefault(item => item.ID == Incident.ID);
                    var damages = db.Damages.Where(item => item.IncidentID == Incident.ID);
                    foreach (var damage in damages)
                        db.Entry(damage).State = EntityState.Deleted;
                    if (incident.Damages == null)
                        incident.Damages = new List<Damage>();
                    else
                        incident.Damages.Clear();

  
                        foreach (var damage in Damages)
                            incident.Damages.Add(damage);

                    incident.IncidentDate = Incident.IncidentDate;
       
                    incident.Description =Incident.Description;
              
                    var automobileID = int.Parse(AutomobileID);
                    incident.AutomobileID = automobileID;

                  

                    var driverid = int.Parse(DriverID);
                    incident.DriverID = driverid;

                    db.SaveChanges();
                }
                return true;
            }
            catch { return false; }
        }
    }
}