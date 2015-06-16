using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class TransitModel
    {
        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$", ErrorMessage = "ساعت بین 0 تا 23")]
        public string HourDelivery { get; set; }


        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$", ErrorMessage = "دقیقه بین 0 تا 59")]
        public string MinuteDelivery { get; set; }

        [Required(ErrorMessage = "زمان برگشت الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$", ErrorMessage = "ساعت بین 0 تا 23")]
        public string HourReturn { get; set; }


        [Required(ErrorMessage = "زمان برگشت الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$", ErrorMessage = "دقیقه بین 0 تا 59")]

        public string MinuteReturn { get; set; }

        public string PersianReturnDate { get; set; }

        public string PersianDeliveryDate { get; set; }


        public Transit Transit { get; set; }

        public List<Attendance> Attendances { get; set; }
        //  public string Attendance { get; set; }

        public List<SelectListItem> Automobiles { get; set; }

        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }
        public string DriverID { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public string DepartmentID { get; set; }


        public List<SelectListItem> CardTraffics { get; set; }
        public string CardTrafficID { get; set; }

        public TransitModel(Controller controller)
        {
            PersianDeliveryDate = PersianDateTime.Now.ToString("yyyy/MM/dd");

            Transit = new DAL.Transit();
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId && item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils.Where(item => item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                CardTraffics.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
                foreach (var trafficCard in db.TrafficCards.Where(item => item.DateExpire >= DateTime.Now))
                    CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
            }
        }

        public TransitModel()
        {
            PersianDeliveryDate = PersianDateTime.Now.ToString("yyyy/MM/dd");

            Transit = new DAL.Transit();
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                foreach (var automobil in db.Automobils.Where(item => item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                    Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });


                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                CardTraffics.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
                foreach (var trafficCard in db.TrafficCards.Where(item => item.DateExpire >= DateTime.Now))
                    CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
            }
        }

        public TransitModel(Transit transit, Controller controller)
        {
            Transit = transit;
            var deliverydatetime = new PersianDateTime(Transit.DeliveryDate);
            PersianDeliveryDate = deliverydatetime.ToString("yyyy/MM/dd HH:mm:ss");
            HourDelivery = deliverydatetime.Hour.ToString();
            MinuteDelivery = deliverydatetime.Minute.ToString();
            var returndatetime = new PersianDateTime(Transit.ReturnDate);
            PersianReturnDate = returndatetime.ToString("yyyy/MM/dd HH:mm:ss");
            HourReturn = returndatetime.Hour.ToString();
            MinuteReturn = returndatetime.Minute.ToString();

            Attendances = new List<Attendance>();
            if (transit.Attendances != null && transit.Attendances.Any())
            {
                foreach (var attendance in transit.Attendances)
                {
                    Attendances.Add(attendance);
                }
            }

            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Automobiles.Add(new SelectListItem { Text = transit.Automobile.Plaque.ToString(), Value = transit.Automobile.ID.ToString() });
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId && item.ID != transit.AutomobileID && item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else
                    foreach (var automobil in db.Automobils.Where(item => item.ID != transit.AutomobileID && item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                var drivers = db.Drivers.Where(item => item.ID != transit.Driver.ID).ToList();
                Drivers.Add(new SelectListItem { Text = transit.Driver.Name.ToString(), Value = transit.Driver.ID.ToString() });
                foreach (var driver in drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                //var departments = db.Departments.Where(item => item.ID != transit.Department.ID).ToList();
                //Departments.Add(new SelectListItem { Text = Transit.Department.Name.ToString(), Value = Transit.Department.ID.ToString() });
                //foreach (var department in departments)
                //    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });
                if (transit.TrafficCard != null)
                {
                    var trafficCards = db.TrafficCards.Where(item => item.ID != transit.TrafficCard.ID).ToList();
                    CardTraffics.Add(new SelectListItem { Text = Transit.TrafficCard.NumberCard.ToString(), Value = Transit.TrafficCard.ID.ToString() });
                    foreach (var trafficCard in trafficCards)
                        CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
                }
            }
        }
    }

    public class TransitDeliveryModel
    {
        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$",ErrorMessage = "ساعت بین 0 تا 23" )]
        public string HourDelivery { get; set; }


        [Required(ErrorMessage = "زمان تحویل الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$",ErrorMessage = "دقیقه بین 0 تا 59")  ]
        public string MinuteDelivery { get; set; }

        public string PersianDeliveryDate { get; set; }


        public string Description { get; set; }

   //     public Transit Transit { get; set; }

        public List<Attendance> Attendances { get; set; }
      
        public List<SelectListItem> Automobiles { get; set; }

        public string AutomobileID { get; set; }

        public List<SelectListItem> Drivers { get; set; }
        public string DriverID { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public string DepartmentID { get; set; }

        public List<SelectListItem> CardTraffics{ get; set; }
        public string CardTrafficID { get; set; }

        public TransitDeliveryModel(Controller controller)
        {
            PersianDeliveryDate = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);
                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId && !item.Transits.Any(transit => transit.ReturnDate == null) && item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

               
                else
                    foreach (var automobil in db.Automobils.Where(item => !item.Transits.Any(transit => transit.ReturnDate == null) && item.AutomobileStatusId==(int) AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                CardTraffics.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
                foreach (var trafficCard in db.TrafficCards.Where(item => item.DateExpire >= DateTime.Now))
                    CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
            }
        }

        public TransitDeliveryModel()
        {
            PersianDeliveryDate = PersianDateTime.Now.ToString("yyyy/MM/dd");
            Automobiles = new List<SelectListItem>();
            Drivers = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var automobil in db.Automobils.Where(item => item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                    Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                foreach (var driver in db.Drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                foreach (var department in db.Departments)
                    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                CardTraffics.Add(new SelectListItem { Text = "انتخاب", Value = (-1).ToString() });
                foreach (var trafficCard in db.TrafficCards.Where(item => item.DateExpire >= DateTime.Now))
                    CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
            }
        }

        public TransitDeliveryModel(Transit transit, Controller controller)    
        {
            var deliverydatetime = new PersianDateTime(transit.DeliveryDate);
            PersianDeliveryDate = deliverydatetime.ToString("yyyy/MM/dd HH:mm:ss");
            var returndatetime = new PersianDateTime(transit.ReturnDate);
            HourDelivery = deliverydatetime.Hour.ToString();
            MinuteDelivery = deliverydatetime.Minute.ToString();
            Attendances = new List<Attendance>();
            if (transit.Attendances != null && transit.Attendances.Any())
            {
                foreach (var attendance in transit.Attendances)
                {
                    Attendances.Add(attendance);
                }
            }
            Automobiles = new List<SelectListItem>();
            Drivers= new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            CardTraffics = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Automobiles.Add(new SelectListItem { Text = transit.Automobile.Plaque.ToString(), Value = transit.Automobile.ID.ToString() });
                var user = db.Users.FirstOrDefault(item => item.UserName == controller.User.Identity.Name);

                if (user.GroupId == (int)GroupModel.User || user.GroupId == (int)GroupModel.StuckReport)
                    foreach (var automobil in db.Automobils.Where(item => item.DepartmentId == user.DepartmentId && item.ID != transit.Automobile.ID && item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });
                else


                    foreach (var automobil in db.Automobils.Where(item => item.ID != transit.Automobile.ID && item.AutomobileStatusId == (int)AutomobileStatusModel.Available))
                        Automobiles.Add(new SelectListItem { Text = automobil.Plaque.ToString(), Value = automobil.ID.ToString() });

                var drivers = db.Drivers.Where(item => item.ID != transit.Driver.ID).ToList();
                Drivers.Add(new SelectListItem { Text = transit.Driver.Name.ToString(), Value = transit.Driver.ID.ToString() });
                foreach (var driver in drivers)
                    Drivers.Add(new SelectListItem { Text = driver.Name.ToString(), Value = driver.ID.ToString() });

                //var departments = db.Departments.Where(item => item.ID != transit.Department.ID).ToList();
                //Departments.Add(new SelectListItem { Text = Transit.Department.Name.ToString(), Value = Transit.Department.ID.ToString() });
                //foreach (var department in departments)
                //    Departments.Add(new SelectListItem { Text = department.Name.ToString(), Value = department.ID.ToString() });

                var trafficCards = db.TrafficCards.Where(item => item.ID != transit.TrafficCard.ID).ToList();
                CardTraffics.Add(new SelectListItem { Text = transit.TrafficCard.NumberCard.ToString(), Value = transit.TrafficCard.ID.ToString() });
                foreach (var trafficCard in trafficCards)
                    CardTraffics.Add(new SelectListItem { Text = trafficCard.NumberCard.ToString(), Value = trafficCard.ID.ToString() });
            }
        }
    }

    public class TransitReturnModel
    {
        public int TransitdId { get; set; }
        
        [Required(ErrorMessage = "زمان برگشت الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-1][0-9])|([0-2][0-3]))$", ErrorMessage = "ساعت بین 0 تا 23")]
        public string HourReturn { get; set; }
        
        [Required(ErrorMessage = "زمان برگشت الزامیست")]
        [RegularExpression(@"^(([0-9])|([0-5][0-9]))$", ErrorMessage = "دقیقه بین 0 تا 59")]
        public string MinuteReturn { get; set; }

        public string PersianReturnDate { get; set; }

        //کیلومتر خودرو بعد از تردد
        [Required(ErrorMessage = "کیلومتر خودرو")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "*")]
        public int? MileagAfterTrip { get; set; }

        public string Automobile { get; set; }

        public TransitReturnModel()
        { }
        public TransitReturnModel(Transit transit, Controller controller)
        {
            PersianReturnDate = PersianDateTime.Now.ToString("yyyy/MM/dd");
            TransitdId = transit.ID;
            Automobile = transit.Automobile.Plaque;
            MileagAfterTrip = 0;
        }
    }
}