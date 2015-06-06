using AutomobilMng.Log;
using AutomobilMng.Models;
using DAL;
using MD.PersianDateTime;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class OilChangeController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "OilChange-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.OilChangeMngMenu;
            ViewBag.Menu = "OilChange";
            var dic = LogAttribute.GetProperties<OilChange>(null, ((int)Subject.OilChangeShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش عملیاتهای سوخت گیری", null, dic.ToArray());
            return View();
        }

        [Authorize(Roles = "OilChange-Show")]
        public ActionResult ShowOilChanges(int automobileid)
        {
            var automobile = applicationDbContext.Automobiles.FirstOrDefault(item => item.ID == automobileid);

            var dic = LogAttribute.GetProperties<Automobile>(automobile, ((int)Subject.OilChangeShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name,
                string.Format( "{0} نمایش عملیاتهای سوخت گیری خودروی ",automobile.Plaque),null, dic.ToArray());
            return PartialView("ShowOilChanges", automobile);
        }

        [Authorize(Roles = "OilChange-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }
        public ActionResult GetOilChanges(JQueryDataTableParamModel param)
        {
            IQueryable<OilChange> oilChanges = applicationDbContext.OilChanges.AsQueryable();
            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                oilChanges = oilChanges.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);
            IEnumerable<OilChange> filtered;
            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);



            if (!string.IsNullOrWhiteSpace(automobile) && automobile != (-1).ToString())
            {
                var automobilid = int.Parse(automobile);
                oilChanges = oilChanges.Where(ivar => ivar.AutomobileID == (automobilid));
            }
            if (!string.IsNullOrWhiteSpace(driver) && driver != (-1).ToString())
            {
                var driverid = int.Parse(driver);
                oilChanges = oilChanges.Where(ivar => ivar.DriverID==(driverid));
            }
  
            filtered = oilChanges.ToList();
            var bSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var bSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var bSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var bSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Automobile, string> orderingFunction = (c => sortColumnIndex == 1 && bSortable_1 ? c.ID.ToString() : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(item=>item.ID);
            else
                filtered = filtered.OrderByDescending(item => item.ID);

            var resultlist = filtered.OrderByDescending(item=>item.ID).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             c.Department.Name,
                            new PersianDateTime(c.ChangeDate).ToString("yyyy/MM/dd"),
                             c.TypeOil,
                             c.Workshop,
                             c.AirFilterChanged.ToString(),
                             c.OilFilterChanged.ToString(),
                             
                             c.ID.ToString()
            };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.OilChanges.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "OilChange-New")]
        public ActionResult New()
        {
            return PartialView("New", new OilChangeModel(this));
        }


        [HttpPost]
        [Authorize(Roles = "OilChange-New")]
        [ValidateAntiForgeryToken]
     //   [LogAttribute(Message = "ثبت عملیات تعویض روغن", ObjectID = ObjectID.OilChangeNew.ToString())]
        public ActionResult New(OilChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Parse(model.PersianChangeDate);
                model.OilChange.ChangeDate = persianDateTime.ToDateTime();

                model.OilChange.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);

                var automobileid = int.Parse(model.AutomobileID);
                model.OilChange.AutomobileID = automobileid;

                var driverid = int.Parse(model.DriverID);
                model.OilChange.DriverID = driverid;

                var departmentid = int.Parse(model.DepartmentID);
                model.OilChange.DepartmentID = departmentid;

                applicationDbContext.OilChanges.Add(model.OilChange);
                applicationDbContext.SaveChanges();

                var dic = LogAttribute.GetProperties<OilChange>(model.OilChange, ((int)Subject.OilChangeNew).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ثبت عملیات تعویض روغن", null, dic.ToArray());

                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }

            var dicfail = LogAttribute.GetProperties<OilChange>(model.OilChange, ((int)Subject.OilChangeNew).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ثبت عملیات تعویض روغن", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {

     
            return PartialView("Search",new OilChangeSearchModel(this));
        }

        [Authorize(Roles = "OilChange-Edit")]
        public ActionResult Edit(int id)
        {
            var OilChanges = applicationDbContext.OilChanges.First(item => item.ID == id);
            var model = new OilChangeModel(OilChanges,this);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "OilChange-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OilChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Parse(model.PersianChangeDate);
                model.OilChange.ChangeDate = persianDateTime.ToDateTime();

                //if (model.OilFilterChanged != null && model.OilFilterChanged == "on")
                //    model.OilChange.OilFilterChanged = true;
                //else
                //    model.OilChange.OilFilterChanged = false;
                //if (model.AirFilterChanged != null && model.AirFilterChanged == "on")
                //    model.OilChange.AirFilterChanged = true;
                //else
                //    model.OilChange.OilFilterChanged = false;


                var automobileid = int.Parse(model.AutomobileID);
                model.OilChange.AutomobileID = automobileid;

                var driverid = int.Parse(model.DriverID);
                model.OilChange.DriverID = driverid;

                var departmentid = int.Parse(model.DepartmentID);
                model.OilChange.DepartmentID = departmentid;


                applicationDbContext.Entry(model.OilChange).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                var messageModel = new MessageModel { Code = 0, Message = "success" };

                var dic = LogAttribute.GetProperties<OilChange>(model.OilChange, ((int)Subject.OilChangeEdit).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name,
             string.Format("بروزرسانی عملیات سوخت گیری"), null, dic.ToArray());

                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            var dicfail = LogAttribute.GetProperties<OilChange>(model.OilChange, ((int)Subject.OilChangeEdit).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name,
         string.Format("بروزرسانی عملیات سوخت گیری"), null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "OilChange-Delete")]
        public ActionResult Delete(int id)
        {
            var OilChanges = applicationDbContext.OilChanges.First(u => u.ID == id);
            var model = new OilChangeModel(OilChanges,this);
            if (OilChanges == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "OilChange-Delete")]
        public ActionResult DeleteConfirmed(OilChangeModel model)
        {
            var OilChanges = applicationDbContext.OilChanges.First(u => u.ID == model.OilChange.ID);
            applicationDbContext.OilChanges.Remove(OilChanges);
            applicationDbContext.SaveChanges();
            var messageModel = new MessageModel { Code = 0, Message = "success" };
            var dic = LogAttribute.GetProperties<OilChange>(model.OilChange, ((int)Subject.OilChangeDelete).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name,
         string.Format("حذف عملیات سوخت گیری"), null, dic.ToArray());

            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (OilChanges == null)
            {
                return HttpNotFound();
            }
            var dicfail = LogAttribute.GetProperties<OilChange>(model.OilChange, ((int)Subject.OilChangeDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name,
         string.Format("حذف عملیات سوخت گیری"), null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });

        }



        [Authorize(Roles = "OilChange-Report")]
        public ActionResult Report(string automobile, string driver)
        {
            ViewBag.automobile = automobile;
            ViewBag.driver = driver;
            return PartialView("Report");
        }
        public ActionResult FromLoadFileReport(string automobile, string driver)
        {
            IQueryable<OilChange> repairs = applicationDbContext.OilChanges.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                repairs = repairs.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<OilChange> filtered;

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
        

            filtered = repairs.ToList();


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//OilChange.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
            report.RegBusinessObject("Data", filtered.Select(item => new OilChangeReportModel
            {
                Automobile = item.Automobile.Plaque,
                CaseChange = (item.OilFilterChanged? "فیلتر هوا":"" )+( item.AirFilterChanged?"فیلتر روغن":""),
                DateChange = new PersianDateTime(item.ChangeDate).ToString("yyyy/MM/dd"),
                Driver = item.Driver.Name,
                Workshop = item.Workshop,
                OilType=item.TypeOil
            }).ToList());
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