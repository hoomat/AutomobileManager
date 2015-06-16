using AutomobilMng.Models;
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
    public class IncidentController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Incident-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.IncidentMngMenu;
            ViewBag.Menu = "Incident";
            return View();
        }

        [Authorize(Roles = "Incident-Show")]
        public ActionResult ShowIncidents(int automobileid)
        {
            var automobile = applicationDbContext.Automobils.FirstOrDefault(item => item.ID == automobileid);
            return PartialView("ShowIncidents", automobile);
        }

        [Authorize(Roles = "Incident-Show")]
        public ActionResult ShowDriverIncidents(int driverid)
        {
            var driver = applicationDbContext.Drivers.FirstOrDefault(item => item.ID == driverid);
            return PartialView("ShowDriverIncidents", driver);
        }

        public ActionResult ShowDepartmentIncidents(int departmentid)
        {
            var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentid);
            return PartialView("ShowDepartmentIncidents", department);
        }
        public ActionResult GetIncidents(JQueryDataTableParamModel param)
        {

            IQueryable<Incident> transits = applicationDbContext.Incidents.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<Incident> filtered;
            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            var fromPersianIncidentDate = Convert.ToString(Request["fromPersianIncidentDate"]);
            var toPersianIncidentDate = Convert.ToString(Request["toPersianIncidentDate"]);

            

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
            if (!string.IsNullOrWhiteSpace(fromPersianIncidentDate))
            {
                var persianIncidentDate = PersianDateTime.Parse(fromPersianIncidentDate ).ToDateTime();
                transits = transits.Where(ivar => ivar.IncidentDate >= persianIncidentDate);
            }
            if (!string.IsNullOrWhiteSpace(toPersianIncidentDate))
            {
               
                var persianIncidentDate = PersianDateTime.Parse(toPersianIncidentDate).ToDateTime();
                transits = transits.Where(ivar => ivar.IncidentDate <= persianIncidentDate);
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

            var resultlist = filtered.OrderByDescending(item=>item.ID).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.ID.ToString(),
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             new PersianDateTime(c.IncidentDate).ToString("yyyy/MM/dd HH:mm:ss"),    
                             c.Description,
                             c.ID.ToString(),
                             //c.Repairs.Count>0? true.ToString():false.ToString()
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Incidents.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Incident-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [Authorize(Roles = "Incident-New")]
        public ActionResult New()
        {
            return PartialView("New", new IncidentModel(this));
        }

        public ActionResult DamageEntryEditor()
        {
            return PartialView("DamageEntryEditor");
        }


        [HttpPost]
        [Authorize(Roles = "Incident-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(IncidentModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Define(User))
                    return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
                else
                    return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search", new IncidentSearchModel(this));
        }

        [Authorize(Roles = "Incident-Edit")]
        public ActionResult Edit(int id)
        {
           var incident = applicationDbContext.Incidents.First(item => item.ID == id);
            var model = new IncidentModel(incident,this);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Incident-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IncidentModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Modify())
                    return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "Incident-Delete")]
        public ActionResult Delete(int id)
        {
            var incident = applicationDbContext.Incidents.First(u => u.ID == id);
            var model = new IncidentModel(incident, this);
            if (incident == null)
                return HttpNotFound();
            return PartialView("Delete", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Incident-Delete")]
        public ActionResult DeleteConfirmed(IncidentModel model)
        {
            if (model.Delete())
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            else
                return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult IncidentRepairs(int? incidentid)
        {
            var repairs = applicationDbContext.Repairs.Where(item=>item.IncidentID==incidentid);
            return PartialView(repairs);
        }



        [Authorize(Roles = "Incident-Report")]
        public ActionResult Report(string automobile, string driver, string fromPersianIncidentDate, string toPersianIncidentDate)
        {
            ViewBag.automobile = automobile;
            ViewBag.driver = driver;
            ViewBag.toPersianIncidentDate = toPersianIncidentDate;
            ViewBag.fromPersianIncidentDate = fromPersianIncidentDate;
            return PartialView("Report");
        }
        public ActionResult FromLoadFileReport(string automobile, string driver , string fromPersianIncidentDate, string toPersianIncidentDate)
        {
            IQueryable<Incident> transits = applicationDbContext.Incidents.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<Incident> filtered;

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
            if (!string.IsNullOrWhiteSpace(fromPersianIncidentDate))
            {
                var persianIncidentDate = PersianDateTime.Parse(fromPersianIncidentDate).ToDateTime();
                transits = transits.Where(ivar => ivar.IncidentDate >= persianIncidentDate);
            }
            if (!string.IsNullOrWhiteSpace(toPersianIncidentDate))
            {
                var persianIncidentDate = PersianDateTime.Parse(toPersianIncidentDate).ToDateTime();
                transits = transits.Where(ivar => ivar.IncidentDate <= persianIncidentDate);
            }
            filtered = transits.ToList();


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//Incident.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
            report.RegBusinessObject("Data", filtered.Select(item => new IncidentReportModel
            {
                Automobile = item.Automobile.Plaque,
                IncidentDate = new PersianDateTime(item.IncidentDate).ToString("yyyy/MM/dd"),
                Driver = item.Driver.Name,
                Description = item.Description
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