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

        public ActionResult GetIncidents(JQueryDataTableParamModel param)
        {

            IQueryable<Incident> transits = applicationDbContext.Incidents.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId != (int)GroupModel.DirectorGeneral)
                transits = transits.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            IEnumerable<Incident> filtered;
            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            

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
            return PartialView("New", new IncidentModel());
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

    }
}