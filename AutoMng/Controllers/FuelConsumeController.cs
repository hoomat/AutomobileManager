using AutomobilMng.Log;
using AutomobilMng.Models;
using DAL;
using MD.PersianDateTime;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class FuelConsumeController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Fuel-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.FuelMngMenu;
            ViewBag.Menu = "Fuel";
            var dic = LogAttribute.GetProperties<FuelConsumeModel>(null, ((int)Subject.FuelShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش سوخت گیری", null, dic.ToArray());
            return View();
        }


        [Authorize(Roles = "Fuel-Show")]
        public ActionResult ShowFuels(int automobileid)
        {
            var automobile = applicationDbContext.Automobiles.Include(a => a.AutomobileClass).FirstOrDefault(item => item.ID == automobileid);
            var dic = LogAttribute.GetProperties<Automobile>(automobile, ((int)Subject.FuelShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش سوخت گیری", null, dic.ToArray());
            return PartialView("ShowFuels", automobile);
        }

        [Authorize(Roles = "Fuel-ReportCompareFuel")]
        public ActionResult FuelCompareConsume()
        {
            ViewBag.MenuShow = AVAResource.Resource.FuelReportCompareMngMenu;
            ViewBag.Menu = "ReportCompareFuel";
            var dic = LogAttribute.GetProperties<FuelConsumeModel>(null, ((int)Subject.FuelReportCompareFuel).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش مقایسه سوخت گیری", null, dic.ToArray());
            return View(new FuelSearchModel(this));
        }

        [Authorize(Roles = "Fuel-ReportCompareFuel")]
        public ActionResult ListCompareConsume()
        {
            return PartialView("ListCompareConsume");
        }

        public ActionResult GetFuels(JQueryDataTableParamModel param)
        {
            IQueryable<FuelConsume> repairs = applicationDbContext.FuelConsumes.AsQueryable();
            IEnumerable<FuelConsume> filtered;
            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                repairs = repairs.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            var fualstation = Convert.ToString(Request["fualstation"]);


            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                repairs = repairs.Where(ivar => ivar.AutomobileID == automobileid);
            }

            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                repairs = repairs.Where(ivar => ivar.DriverID == driverid);
            }
            if (!string.IsNullOrWhiteSpace(fualstation))
                repairs = repairs.Where(ivar => ivar.FualStation.Contains(fualstation));

            filtered = repairs.ToList();
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

            var resultlist = filtered.OrderByDescending(item=>item.ID).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             c.Department.Name,
                             new PersianDateTime(c.RefuelDate).ToString("yyyy/MM/dd HH:mm:ss"), 
                             c.VolumeFuel.ToString(),
                             c.AmountPaid.ToString(),
                             c.Distance.ToString(),
                             c.PaymentType.Type,
                            (c.FuelCard ==null) ? "" : c.FuelCard.Number ,
                             c.FualStation,
                             c.Description,
                             c.ID.ToString()
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.FuelConsumes.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompareFuels(JQueryDataTableParamModel param)
        {
            IQueryable<FuelConsume> fuelConsumes = applicationDbContext.FuelConsumes.AsQueryable();
           
            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                fuelConsumes = fuelConsumes.Where(ivar => ivar.Automobile.ID == identityUser.DepartmentId);

            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            var fualstation = Convert.ToString(Request["fualstation"]);


            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                fuelConsumes = fuelConsumes.Where(ivar => ivar.AutomobileID == automobileid);
            }


            List<FuelCompareConsumeReportModel> filtered =
                (from row in fuelConsumes 
                 group row by new { row.Automobile } into g
                 select new FuelCompareConsumeReportModel
                 {
                     RealFualConsume=(g.Sum(x => x.VolumeFuel)/g.Sum(x => x.Distance)).ToString(),
                     FualConsume = g.Key.Automobile.FualConsume,
                     AllVolumeFuel = g.Sum(x => x.VolumeFuel).ToString(),
                     AllAmountPaid = g.Sum(x => x.AmountPaid).ToString(),
                     Automobile = g.Key.Automobile.Plaque,
                     Distance = g.Sum(x => x.Distance).ToString()
                 }).ToList();
            
            var resultlist = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile.ToString(),
                             c.AllAmountPaid,
                             c.AllVolumeFuel,
                             c.Distance,
                              (c.FualConsume ==null) ? "" : c.FualConsume ,
                             c.RealFualConsume.ToString(),
                             
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.FuelConsumes.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Repair-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [Authorize(Roles = "Fuel-New")]
        public ActionResult New()
        {
            return PartialView("New", new FuelConsumeModel(this));
        }

        [HttpPost]
        [Authorize(Roles = "Fuel-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(FuelConsumeModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDeliveryDate = PersianDateTime.Parse(model.PersianRefuelDate + " " + model.HourRefuelDate + ":" + model.MinuteRefuelDate + ":00");
                model.FuelConsume.RefuelDate = persianDeliveryDate.ToDateTime();

                model.FuelConsume.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);

                var automobileID = int.Parse(model.AutomobileID);
                //var automobile = applicationDbContext.Automobiles.FirstOrDefault(item => item.ID == id);
                model.FuelConsume.AutomobileID = automobileID;

                var driverid = int.Parse(model.DriverID);
                // var driver = applicationDbContext.Drivers.FirstOrDefault(item => item.ID == driverid);
                model.FuelConsume.DriverID = driverid;

                var departmentid = int.Parse(model.DepartmentID);
                //  var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentid);
                model.FuelConsume.DepartmentID = departmentid;

                if (!string.IsNullOrWhiteSpace(model.FuelCardID) && model.FuelCardID != (-1).ToString())
                {
                    var fuelcardid = int.Parse(model.FuelCardID);
                    // var fuelcard = applicationDbContext.FuelCards.FirstOrDefault(item => item.ID == fuelcardid);
                    model.FuelConsume.FuelCardID = fuelcardid;
                }

                if (!string.IsNullOrWhiteSpace(model.FualTypeID) && model.FualTypeID != (-1).ToString())
                {
                    var fualTypeID = int.Parse(model.FualTypeID);
                    // var fuelcard = applicationDbContext.FuelCards.FirstOrDefault(item => item.ID == fuelcardid);
                    model.FuelConsume.FualTypeID = fualTypeID;
                }

                //  var paymentType= applicationDbContext.PaymentTypes.FirstOrDefault(item => item.ID == model.PaymentTypeID);
                model.FuelConsume.PaymentTypeID = model.PaymentTypeID;


                var lastTransit = applicationDbContext.FuelConsumes.Where(item =>  item.AutomobileID == automobileID).OrderByDescending(item => item.ID).Take(1);
                if (lastTransit.Any())
                    model.FuelConsume.Distance = model.FuelConsume.Mileag - lastTransit.FirstOrDefault().Mileag;
                else
                    model.FuelConsume.Distance = model.FuelConsume.Mileag;
                // transit.Automobile.Distance = model.MileagAfterTrip.Value;

                try
                {
                    applicationDbContext.FuelConsumes.Add(model.FuelConsume);
                    applicationDbContext.SaveChanges();
                    var dic = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelNew).ToString(), "success");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف سوخت گیری", null, dic.ToArray());
                }
                catch (DbEntityValidationException ex)
                {
                    var dicfail = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelNew).ToString(), "fail");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف سوخت گیری", null, dicfail.ToArray());
                }
                var dicfail0 = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelNew).ToString(), "fail");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف سوخت گیری", null, dicfail0.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });

            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search", new FuelSearchModel(this));
        }
        public ActionResult SearchCompareConsume()
        {
            return PartialView("SearchCompareConsume", new FuelSearchModel(this));
        }

        [Authorize(Roles = "Fuel-Edit")]
        public ActionResult Edit(int id)
        {
            var fuelConsume = applicationDbContext.FuelConsumes.First(item => item.ID == id);
            var model = new FuelConsumeModel(fuelConsume, this);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Fuel-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FuelConsumeModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDeliveryDate = PersianDateTime.Parse(model.PersianRefuelDate + " " + model.HourRefuelDate + ":" + model.MinuteRefuelDate + ":00");
                model.FuelConsume.RefuelDate = persianDeliveryDate.ToDateTime();

                var automobileID = int.Parse(model.AutomobileID);
                //var automobile = applicationDbContext.Automobiles.FirstOrDefault(item => item.ID == id);
                model.FuelConsume.AutomobileID = automobileID;

                var driverid = int.Parse(model.DriverID);
                // var driver = applicationDbContext.Drivers.FirstOrDefault(item => item.ID == driverid);
                model.FuelConsume.DriverID = driverid;

                var departmentid = int.Parse(model.DepartmentID);
                //  var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentid);
                model.FuelConsume.DepartmentID = departmentid;

                if (!string.IsNullOrWhiteSpace(model.FuelCardID) && model.FuelCardID != (-1).ToString())
                {
                    var fuelcardid = int.Parse(model.FuelCardID);
                    // var fuelcard = applicationDbContext.FuelCards.FirstOrDefault(item => item.ID == fuelcardid);
                    model.FuelConsume.FuelCardID = fuelcardid;
                }

                var lastTransit = applicationDbContext.FuelConsumes.Where(item => item.ID < model.FuelConsume.ID && item.AutomobileID == automobileID).OrderByDescending(item => item.ID).Take(1);
                if (lastTransit.Any())
                    model.FuelConsume.Distance = model.FuelConsume.Mileag - lastTransit.FirstOrDefault().Mileag;
                else
                    model.FuelConsume.Distance = model.FuelConsume.Mileag;
                // transit.Automobile.Distance = model.MileagAfterTrip.Value;

                if (!string.IsNullOrWhiteSpace(model.FualTypeID) && model.FualTypeID != (-1).ToString())
                {
                    var fualTypeID = int.Parse(model.FualTypeID);
                    // var fuelcard = applicationDbContext.FuelCards.FirstOrDefault(item => item.ID == fuelcardid);
                    model.FuelConsume.FualTypeID = fualTypeID;
                }
                //  var paymentType= applicationDbContext.PaymentTypes.FirstOrDefault(item => item.ID == model.PaymentTypeID);
                model.FuelConsume.PaymentTypeID = model.PaymentTypeID;
                applicationDbContext.Entry(model.FuelConsume).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                var dic = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelEdit).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی سوخت گیری", null, dic.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            var dicfail = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelEdit).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی سوخت گیری", null, dicfail.ToArray());
            return PartialView("Edit", model);
        }

        [Authorize(Roles = "Fuel-Delete")]
        public ActionResult Delete(int id)
        {
            var fuelconsume = applicationDbContext.FuelConsumes.First(u => u.ID == id);
            var model = new FuelConsumeModel(fuelconsume, this);
            if (fuelconsume == null)
                return HttpNotFound();
            return PartialView("Delete", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fuel-Delete")]
        public ActionResult DeleteConfirmed(FuelConsumeModel model)
        {
            var fuelConsume = applicationDbContext.FuelConsumes.First(u => u.ID == model.FuelConsume.ID);
            applicationDbContext.FuelConsumes.Remove(fuelConsume);
            applicationDbContext.SaveChanges();
            var dic = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelDelete).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف سوخت گیری", null, dic.ToArray());

            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            var dicfail = LogAttribute.GetProperties<FuelConsumeModel>(model, ((int)Subject.FuelDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف سوخت گیری", null, dicfail.ToArray());
            return PartialView("Delete", model);

        }



        [Authorize(Roles = "Transit-Report")]
        public ActionResult Report(string automobile, string driver, string fualstation)
        {
            ViewBag.automobile = automobile;
            ViewBag.driver = driver;
            ViewBag.fualstation = fualstation;
            var dic = LogAttribute.GetProperties<FuelConsumeModel>(null, ((int)Subject.FuelReport).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "گزارش سوخت گیری", null, dic.ToArray());
            return PartialView("Report");
        }

          [Authorize(Roles = "Transit-Report")]
        public ActionResult ReportCompareConsume(string automobile)
        {
            ViewBag.automobile = automobile;
            return PartialView("ReportCompareConsume");
        }

        
        ////Reporter actions
        public ActionResult FromLoadFileReport(string automobile, string driver, string fualstation)
        {
            IQueryable<FuelConsume> fuelConsumes = applicationDbContext.FuelConsumes.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                fuelConsumes = fuelConsumes.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<FuelConsume> filtered;

            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                fuelConsumes = fuelConsumes.Where(ivar => ivar.AutomobileID == automobileid);
            }

            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                fuelConsumes = fuelConsumes.Where(ivar => ivar.DriverID == driverid);
            }
            if (!string.IsNullOrWhiteSpace(fualstation))
                fuelConsumes = fuelConsumes.Where(ivar => ivar.FualStation.Contains(fualstation));


            filtered = fuelConsumes.ToList();


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//FuelConsume.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
            report.RegBusinessObject("Data", filtered.Select(item => new FuelConsumeReportModel
            {
                Automobile = item.Automobile.Plaque,
                AmountPaid = item.AmountPaid,
                Department = item.Department.Name,
                RefuelDate = new PersianDateTime(item.RefuelDate).ToString("yyyy/MM/dd"),
                Driver = item.Driver.Name,
                FualStation = item.FualStation,
                PaymentType = item.PaymentType.Type,
                VolumeFuel = item.VolumeFuel
            }).ToList());
            return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
        }

        public ActionResult FromLoadCompareConsume(string automobile)
        {
            IQueryable<FuelConsume> fuelConsumes = applicationDbContext.FuelConsumes.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                fuelConsumes = fuelConsumes.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

         

            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobileid = int.Parse(automobile);
                fuelConsumes = fuelConsumes.Where(ivar => ivar.AutomobileID == automobileid);
            }

            List<FuelCompareConsumeReportModel> filtered =
         (from row in fuelConsumes
          group row by new { row.Automobile } into g
          select new FuelCompareConsumeReportModel
          {
              RealFualConsume = (g.Sum(x => x.VolumeFuel) / g.Sum(x => x.Distance)).ToString(),
              FualConsume = g.Key.Automobile.FualConsume,
              AllVolumeFuel = g.Sum(x => x.VolumeFuel).ToString(),
              AllAmountPaid = g.Sum(x => x.AmountPaid).ToString(),
              Automobile = g.Key.Automobile.Plaque,
              Distance = g.Sum(x => x.Distance).ToString()
          }).ToList();
            


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//FuelCompareConsume.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
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
    }
}