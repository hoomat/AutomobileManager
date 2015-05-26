﻿using AutomobilMng.Models;
using DAL;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using MD.PersianDateTime;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class TransitController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Transit-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.TransitMngMenu;
            ViewBag.Menu = "Transit";
            return View();
        }

        public ActionResult GetTransits(JQueryDataTableParamModel param)
        {
            IQueryable<Transit> transits = applicationDbContext.Transits.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId != (int)GroupModel.DirectorGeneral)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<Transit> filtered;
            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            var department = Convert.ToString(Request["department"]);

            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                transits = transits.Where(ivar => ivar.AutomobileID == automobileid);
            }

            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                transits = transits.Where(ivar => ivar.DriverID == driverid);
            }
            if (!string.IsNullOrWhiteSpace(department) && department != (-1).ToString())
            {
                var departmentid = int.Parse(department);
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == departmentid);
            }
            filtered = transits.ToList();
            var bSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var bSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var bSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var bSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Automobile, string> orderingFunction = (c => sortColumnIndex == 1 && bSortable_1 ? c.ID.ToString() : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(item => item.ID);
            else
                filtered = filtered.OrderByDescending(item => item.ID);

            var resultlist = filtered.OrderByDescending(item => item.ID).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             c.Automobile.Department.Name,
                             new PersianDateTime(c.DeliveryDate).ToString("yyyy/MM/dd HH:mm:ss"),  
                             new PersianDateTime(c.ReturnDate).ToString("yyyy/MM/dd HH:mm:ss"),   
                             c.MileagAfterTrip.ToString(),
                             c.Distance.ToString(),
                              c.Description,
                             c.ID.ToString()
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Transits.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTransitReports(JQueryDataTableParamModel param)
        {
            IQueryable<Transit> transits = applicationDbContext.Transits.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId != (int)GroupModel.DirectorGeneral)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<Transit> filtered;
            var automobile = Convert.ToString(Request["automobile"]);
              var driver = Convert.ToString(Request["driver"]);
            var department = Convert.ToString(Request["department"]);
            var trafficcard = Convert.ToString(Request["trafficcard"]);
            var fromDeliveryDate = Convert.ToString(Request["fromDeliveryDate"]);
              var toDeliveryDate = Convert.ToString(Request["toDeliveryDate"]);
            var fromReturnDate = Convert.ToString(Request["fromReturnDate"]);
            var toReturnDate = Convert.ToString(Request["toReturnDate"]);
            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                transits = transits.Where(ivar => ivar.AutomobileID == automobileid);
            }

            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                transits = transits.Where(ivar => ivar.DriverID == driverid);
            }
            if (!string.IsNullOrWhiteSpace(department) && department != (-1).ToString())
            {
                var departmentid = int.Parse(driver);
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == departmentid);
            }
            if (!string.IsNullOrWhiteSpace(trafficcard) && trafficcard != (-1).ToString())
            {
                var trafficcardid = int.Parse(trafficcard);
                transits = transits.Where(ivar => ivar.TrafficCardID == trafficcardid);
            }
            if (!string.IsNullOrWhiteSpace(fromDeliveryDate))
            {
                var date=PersianDateTime.Parse(fromDeliveryDate).ToDateTime();
                transits = transits.Where(ivar => ivar.DeliveryDate >= date);
            }
            if (!string.IsNullOrWhiteSpace(toDeliveryDate))
            {
                var date = PersianDateTime.Parse(toDeliveryDate).ToDateTime();
                transits = transits.Where(ivar => ivar.DeliveryDate <= date);
            }
            if (!string.IsNullOrWhiteSpace(fromDeliveryDate))
            {
                var date = PersianDateTime.Parse(fromDeliveryDate).ToDateTime();
                transits = transits.Where(ivar => ivar.ReturnDate >= date);
            }
            if (!string.IsNullOrWhiteSpace(toReturnDate))
            {
                var date = PersianDateTime.Parse(toReturnDate).ToDateTime();
                transits = transits.Where(ivar => ivar.ReturnDate <= date);
            }
            filtered = transits.ToList();
            var bSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var bSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var bSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var bSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Automobile, string> orderingFunction = (c => sortColumnIndex == 1 && bSortable_1 ? c.ID.ToString() : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(item => item.ID);
            else
                filtered = filtered.OrderByDescending(item => item.ID);

            var resultlist = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                           
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             c.Automobile.Department.Name,
                            new PersianDateTime(c.DeliveryDate).ToString("yyyy/MM/dd HH:mm:ss"),  
                           new PersianDateTime(c.ReturnDate).ToString("yyyy/MM/dd HH:mm:ss"),   
                             c.Description,
                             c.MileagAfterTrip.ToString(),
                             c.Distance.ToString(),
                           
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Transits.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTransitDistanceReports(JQueryDataTableParamModel param)
        {
            IQueryable<Transit> transits = applicationDbContext.Transits.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId != (int)GroupModel.DirectorGeneral)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            List<TransitDistanceModel> filtered =
                (from row in applicationDbContext.Transits
                 group row by new { row.Automobile } into g
                 select new TransitDistanceModel()
                 {
                     Automobile = g.Key.Automobile.Plaque,
                     Distance = g.Sum(x => x.Distance.Value)
                 }).OrderByDescending(item=>item.Distance).ToList();

            var resultlist = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile,
                             c.Distance.ToString(),
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Transits.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Transit-UnDelivery-Show")]
        public ActionResult TransitUnDelivery()
        {
            ViewBag.MenuShow = AVAResource.Resource.TransitMngMenu;
            ViewBag.Menu = "TransitUnDelivery";
            return View();
        }

        public ActionResult GetUnDeliveryTransits(JQueryDataTableParamModel param)
        {
            IQueryable<Transit> transits = applicationDbContext.Transits.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId != (int)GroupModel.DirectorGeneral)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            transits = transits.Where(ivar => ivar.ReturnDate == null);
            IEnumerable<Transit> filtered;
            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            var department = Convert.ToString(Request["department"]);

            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                transits = transits.Where(ivar => ivar.AutomobileID == automobileid);
            }

            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                transits = transits.Where(ivar => ivar.DriverID == driverid);
            }
            if (!string.IsNullOrWhiteSpace(department) && department != (-1).ToString())
            {
                var departmentid = int.Parse(department);
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == departmentid);
            }
            filtered = transits.ToList();
            var bSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var bSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var bSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var bSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Automobile, string> orderingFunction = (c => sortColumnIndex == 1 && bSortable_1 ? c.ID.ToString() : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(item => item.ID);
            else
                filtered = filtered.OrderByDescending(item => item.ID);

            var resultlist = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             c.Automobile.Department.Name,
                             new PersianDateTime(c.DeliveryDate).ToString("yyyy/MM/dd HH:mm:ss"),  
                             new PersianDateTime(c.ReturnDate).ToString("yyyy/MM/dd HH:mm:ss"),   
                             c.MileagAfterTrip.ToString(),
                             c.Distance.ToString(),
                              c.Description,
                             c.ID.ToString()
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Transits.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Transit-Report")]
        public ActionResult ReportTransit()
        {
            ViewBag.MenuShow = AVAResource.Resource.TransitReportMngMenu;
            ViewBag.Menu = "ReportTransit";
            return View(new TransitSearchModel(this));
        }

        [Authorize(Roles = "Transit-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        public ActionResult AttendanceEntryEditor()
        {
            return PartialView("AttendanceEntryEditor");
        }

        [Authorize(Roles = "Transit-Delivery")]
        public ActionResult DeliveryTransit()
        {
            return PartialView("DeliveryTransit", new TransitDeliveryModel());
        }

        [HttpPost]
        [Authorize(Roles = "Transit-Delivery")]
        [ValidateAntiForgeryToken]
        public ActionResult DeliveryTransit(TransitDeliveryModel model)
        {
            if (ModelState.IsValid)
            {
                Transit transit = new Transit();
                var persianDeliveryDate = PersianDateTime.Parse(model.PersianDeliveryDate+" "+model.HourDelivery+":"+model.MinuteDelivery+":00");
                transit.DeliveryDate = persianDeliveryDate.ToDateTime();

                transit.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);

                transit.Attendances = new List<Attendance>();

                if (model.Attendances != null)
                    foreach (var attendance in model.Attendances)
                        transit.Attendances.Add(attendance);

                var automobileID = int.Parse(model.AutomobileID);
                transit.AutomobileID = automobileID;

                //var lastTransit = applicationDbContext.Transits.OrderByDescending(item => item.ID).Take(1); 
                //if (lastTransit.Any())
                //    transit.Distance = transit.MileagAfterTrip - lastTransit.FirstOrDefault().MileagAfterTrip;
                //else
                //    model.Transit.Distance = model.Transit.MileagAfterTrip;

                var driverid = int.Parse(model.DriverID);
                transit.DriverID = driverid;

                if (string.IsNullOrWhiteSpace(model.CardTrafficID) && model.CardTrafficID != "-1")
                {
                    var cardTrafficID = int.Parse(model.CardTrafficID);
                    transit.TrafficCardID = cardTrafficID;
                }

                applicationDbContext.Transits.Add(transit);
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
    
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }




        [Authorize(Roles = "Transit-Return")]
        public ActionResult ReturnTransit(int id)
        {
            var transit = applicationDbContext.Transits.First(item => item.ID == id);
            var transitmodel=new TransitReturnModel(transit,this);
            return PartialView("ReturnTransit", transitmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Transit-Return")]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnTransit(TransitReturnModel model)
        {
            if (ModelState.IsValid)
            {
                var transit = applicationDbContext.Transits.FirstOrDefault(item => item.ID == model.TransitdId);
                var persianReturnDate = PersianDateTime.Parse(model.PersianReturnDate + " " + model.HourReturn + ":" + model.MinuteReturn + ":00");
                transit.ReturnDate = persianReturnDate.ToDateTime();
                transit.MileagAfterTrip = model.MileagAfterTrip;
                if (model.MileagAfterTrip != null)
                {
                    var lastTransit = applicationDbContext.Transits.Where(item => item.ID < model.TransitdId && item.ReturnDate != null).OrderByDescending(item => item.ID).Take(1);
                    if (lastTransit.Any())
                        transit.Distance = model.MileagAfterTrip.Value - lastTransit.FirstOrDefault().MileagAfterTrip.Value;
                    else
                        transit.Distance = model.MileagAfterTrip.Value;
                   // transit.Automobile.Distance = model.MileagAfterTrip.Value;
                }
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }




        //public ActionResult Exit(TransitModel model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var persianDeliveryDate = PersianDateTime.Parse(model.PersianDeliveryDate + " " + model.HourDelivery + ":" + model.MinuteDelivery + ":00");
        //        model.Transit.DeliveryDate = persianDeliveryDate.ToDateTime();

        //        var persianReturnDate = PersianDateTime.Parse(model.PersianReturnDate + " " + model.HourReturn + ":" + model.MinuteReturn + ":00");
        //        model.Transit.ReturnDate = persianReturnDate.ToDateTime();

        //        model.Transit.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);

        //        model.Transit.Attendances = new List<Attendance>();

        //        foreach (var attendance in model.Attendances)
        //            model.Transit.Attendances.Add(attendance);

        //        var automobileID = int.Parse(model.AutomobileID);
        //        model.Transit.AutomobileID = automobileID;

        //        var lastTransit = applicationDbContext.Transits.OrderByDescending(item => item.ID).Take(1);
        //        if (lastTransit.Any())
        //            model.Transit.Distance = model.Transit.MileagAfterTrip - lastTransit.FirstOrDefault().MileagAfterTrip;
        //        else
        //            model.Transit.Distance = model.Transit.MileagAfterTrip;

        //        var driverid = int.Parse(model.DriverID);
        //        model.Transit.DriverID = driverid;


        //        var cardTrafficID = int.Parse(model.CardTrafficID);
        //        model.Transit.TrafficCardID = cardTrafficID;

        //        applicationDbContext.Transits.Add(model.Transit);
        //        applicationDbContext.SaveChanges();
        //        return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });

        //    }
        //    return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        //}
        public ActionResult Search()
        {
            return PartialView("Search",new TransitSearchModel(this));
        }

        public ActionResult SearchReport()
        {
            return PartialView("SearchReport",new TransitSearchModel(this));
        }

        [Authorize(Roles = "Transit-Edit")]
        public ActionResult Edit(int id)
        {
            var transit = applicationDbContext.Transits.First(item => item.ID == id);
            var model = new TransitModel(transit,this);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Transit-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TransitModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDeliveryDate = PersianDateTime.Parse(model.PersianDeliveryDate + " " + model.HourDelivery + ":" + model.MinuteDelivery + ":00");
                model.Transit.DeliveryDate = persianDeliveryDate.ToDateTime();

                //var persianReturnDate = PersianDateTime.Parse(model.PersianReturnDate + " " + model.HourReturn + ":" + model.MinuteReturn + ":00");
                //model.Transit.ReturnDate = persianReturnDate.ToDateTime();

                var transit = applicationDbContext.Transits.FirstOrDefault(item => item.ID == model.Transit.ID);
                var attendances = applicationDbContext.Attendancea.Where(item => item.Transit.ID == model.Transit.ID);
                foreach (var attendance in attendances)
                    applicationDbContext.Entry(attendance).State = EntityState.Deleted;
                if (transit.Attendances == null)
                    transit.Attendances = new List<Attendance>();
                else
                    transit.Attendances.Clear();

                if (model.Attendances != null)
                {
                    transit.Attendances = new List<Attendance>();

                    foreach (var attendance in model.Attendances)
                        transit.Attendances.Add(attendance);
                }

                transit.DeliveryDate = model.Transit.DeliveryDate;
                transit.ReturnDate = model.Transit.ReturnDate;
                transit.Description = model.Transit.Description;
                transit.MileagAfterTrip = model.Transit.MileagAfterTrip;
                var automobileID = int.Parse(model.AutomobileID);
                transit.AutomobileID = automobileID;

                if (model.Transit.MileagAfterTrip != null)
                {
                    var lastTransit = applicationDbContext.Transits.Where(item => item.ID < model.Transit.ID).OrderByDescending(item => item.ID).Take(1);
                    if (lastTransit.Any())
                        transit.Distance = model.Transit.MileagAfterTrip.Value - lastTransit.FirstOrDefault().MileagAfterTrip.Value;
                    else
                        transit.Distance = model.Transit.MileagAfterTrip.Value;
                }
                if (model.CardTrafficID !=null && string.IsNullOrWhiteSpace(model.CardTrafficID) && model.CardTrafficID != "-1")
                {
                    var cardTrafficid = int.Parse(model.CardTrafficID);
                    transit.TrafficCardID = cardTrafficid;
                }
                var driverid = int.Parse(model.DriverID);
                transit.DriverID = driverid;

                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "Transit-Delete")]
        public ActionResult Delete(int id)
        {
            var transit = applicationDbContext.Transits.First(u => u.ID == id);
            var model = new TransitModel(transit,this);
            if (transit == null)
                return HttpNotFound();
            return PartialView("Delete", model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Transit-Delete")]
        public ActionResult DeleteConfirmed(TransitModel model)
        {
            var transit = applicationDbContext.Transits.First(u => u.ID == model.Transit.ID);
            applicationDbContext.Transits.Remove(transit);
            applicationDbContext.SaveChanges();
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (transit == null)
            {
                return HttpNotFound();
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
           
        }


        [Authorize(Roles = "Transit-Report")]
        public ActionResult Report(string automobile, string driver, string department,
            string trafficcard, string fromDeliveryDate, string toDeliveryDate, string fromReturnDate, string toReturnDate)
        {
            ViewBag.automobile = automobile;
            ViewBag.driver = driver;
            ViewBag.department = department;
            ViewBag.trafficcard = trafficcard;
            ViewBag.fromDeliveryDate = fromDeliveryDate;
            ViewBag.toDeliveryDate = toDeliveryDate;
            ViewBag.fromReturnDate = fromReturnDate;
            ViewBag.toReturnDate = toReturnDate;
            return PartialView("Report");
        }


        [Authorize(Roles = "Transit-Report")]
        public ActionResult ReportDistance()
        {
            ViewBag.MenuShow = AVAResource.Resource.TransitReportDisMngMenu;
            ViewBag.Menu = "ReportDistance";
            return View();
        }

        ////Reporter actions
        public ActionResult FromLoadFileReport(string automobile, string driver, string department,
            string trafficcard, string fromDeliveryDate, string toDeliveryDate, string fromReturnDate, string toReturnDate)
        {
            IQueryable<Transit> transits = applicationDbContext.Transits.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            transits = transits.Where(ivar => ivar.IdentityUser.Id == identityUser.Id);

            IEnumerable<Transit> filtered;
    
            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                transits = transits.Where(ivar => ivar.AutomobileID == automobileid);
            }

            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                transits = transits.Where(ivar => ivar.DriverID == driverid);
            }
            if (!string.IsNullOrWhiteSpace(department) && department != (-1).ToString())
            {
                var departmentid = int.Parse(driver);
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == departmentid);
            }
            if (!string.IsNullOrWhiteSpace(trafficcard) && trafficcard != (-1).ToString())
            {
                var trafficcardid = int.Parse(trafficcard);
                transits = transits.Where(ivar => ivar.TrafficCardID == trafficcardid);
            }
            if (!string.IsNullOrWhiteSpace(fromDeliveryDate))
            {
                var date = PersianDateTime.Parse(fromDeliveryDate).ToDateTime();
                transits = transits.Where(ivar => ivar.DeliveryDate >= date);
            }
            if (!string.IsNullOrWhiteSpace(toDeliveryDate))
            {
                var date = PersianDateTime.Parse(toDeliveryDate).ToDateTime();
                transits = transits.Where(ivar => ivar.DeliveryDate <= date);
            }
            if (!string.IsNullOrWhiteSpace(fromDeliveryDate))
            {
                var date = PersianDateTime.Parse(fromDeliveryDate).ToDateTime();
                transits = transits.Where(ivar => ivar.ReturnDate >= date);
            }
            if (!string.IsNullOrWhiteSpace(toReturnDate))
            {
                var date = PersianDateTime.Parse(toReturnDate).ToDateTime();
                transits = transits.Where(ivar => ivar.ReturnDate <= date);
            }
            filtered = transits.ToList();


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//Transit.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
            report.RegBusinessObject("Data", aut.Select(item => new TransitReportModel
            {
                Automobile = item.Automobile.Plaque,
                DeliveryDate = new PersianDateTime(item.DeliveryDate).ToString("yyyy/MM/dd"),
                ReturnDate = new PersianDateTime(item.ReturnDate).ToString("yyyy/MM/dd"),
                Distance = item.Distance.Value,
                Driver = item.Driver.Name,
                TrafficCard = item.TrafficCard.NumberCard,
                Department=item.Automobile.Department.Name
            }));
            return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
        }

        public ActionResult LoadRepotDistance()
        {
            IQueryable<Transit> transits = applicationDbContext.Transits.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId != (int)GroupModel.DirectorGeneral)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            List<TransitDistanceModel> filtered =
                (from row in applicationDbContext.Transits
                 group row by new { row.Automobile } into g
                 select new TransitDistanceModel()
                 {
                     Automobile = g.Key.Automobile.Plaque,
                     Distance = g.Sum(x => x.Distance.Value)
                 }).OrderByDescending(item => item.Distance).ToList();

            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//TransitDistance.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            
            report.RegBusinessObject("Data", filtered);
            return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
        }

        public ActionResult PrintReport()
        {

            return StiMvcViewer.PrintReportResult(this.HttpContext);

        }

        public ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult(this.HttpContext);
        }


        public ActionResult ChartTransitDepartmentDistance()
        {
            List<Series> Series = new List<Series>();
            List<object> listdata = new List<object>();
            List<TransitDepartmentDistanceModel> filtered =
              (from row in applicationDbContext.Transits
               group row by new { row.Automobile.Department } into grouped
               select new TransitDepartmentDistanceModel()
               {
                   Department = grouped.Key.Department.Name,
                   Distance = grouped.Sum(x => x.Distance.Value)
               }).ToList();

            foreach (var category in filtered)
                listdata.Add(category.Distance);



            Series.Add(new Series { Name = "مسافت ", Data = new Data(listdata.ToArray()) });
            Highcharts chart = new Highcharts("chart") { }

                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                  .SetTitle(new Title { Text = "نمودار مسافت طی شده اداره ها" })
                  .SetXAxis(new XAxis { Categories = filtered.Select(item => item.Department).ToArray() })
                  .SetYAxis(new YAxis
                  {
                      AllowDecimals=false,
                      Min = 0,
                      Title = new YAxisTitle { Text = "بر اساس تعداد" }
                  })
                  .SetLegend(new Legend
                  {

                      Rtl = true,
                      Layout = Layouts.Vertical,
                      Align = HorizontalAligns.Left,
                      VerticalAlign = VerticalAligns.Top,
                      X = 100,
                      Y = 70,
                      // Floating = true,
                      BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                      Shadow = true
                  })
                  .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.x +': '+ this.y ; }" })
                  .SetPlotOptions(new PlotOptions
                  {
                      Column = new PlotOptionsColumn
                      {
                          PointPadding = 0.2,
                          BorderWidth = 0
                      }
                  })
                  .SetSeries(
                  Series.ToArray()
                //  new[]
                //{
                //    new Series { Name = "تحویل دایمی", Data = new Data(new object[] { 49.9, 71.5 }) },
                //   // new Series { Name = "تحویل موقت", Data = new Data(new object[] { 48.9, 38.8}) },
                //}
                );


            return PartialView(chart);
        }

        public ActionResult ChartTransitDriverDistance()
        {
            List<Series> Series = new List<Series>();
            List<object> listdata = new List<object>();
            List<TransitDriverDistanceModel> filtered =
              (from row in applicationDbContext.Transits
               group row by new { row.Driver } into grouped
               select new TransitDriverDistanceModel()
               {
                   Driver = grouped.Key.Driver.Name,
                   Distance = grouped.Sum(x => x.Distance.Value)
               }).ToList();

            foreach (var category in filtered)
                listdata.Add(category.Distance);



            Series.Add(new Series { Name = "مسافت ", Data = new Data(listdata.ToArray()) });
            Highcharts chart = new Highcharts("chart") { }

                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                  .SetTitle(new Title { Text = "نمودار مسافت طی شده راننده ها" })
                  .SetXAxis(new XAxis { Categories = filtered.Select(item => item.Driver).ToArray() })
                  .SetYAxis(new YAxis
                  {
                      AllowDecimals=false,
                      Min = 0,
                      Title = new YAxisTitle { Text = "بر اساس تعداد" }
                  })
                  .SetLegend(new Legend
                  {

                      Rtl = true,
                      Layout = Layouts.Vertical,
                      Align = HorizontalAligns.Left,
                      VerticalAlign = VerticalAligns.Top,
                      X = 100,
                      Y = 70,
                      // Floating = true,
                      BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                      Shadow = true
                  })
                  .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.x +': '+ this.y ; }" })
                  .SetPlotOptions(new PlotOptions
                  {
                      Column = new PlotOptionsColumn
                      {
                          PointPadding = 0.2,
                          BorderWidth = 0
                      }
                  })
                  .SetSeries(
                  Series.ToArray()
                //  new[]
                //{
                //    new Series { Name = "تحویل دایمی", Data = new Data(new object[] { 49.9, 71.5 }) },
                //   // new Series { Name = "تحویل موقت", Data = new Data(new object[] { 48.9, 38.8}) },
                //}
                );


            return PartialView(chart);
        }
    }
}