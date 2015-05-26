using AutomobilMng.Models;
using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class DriverController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Driver-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.Menu_Driver;
            ViewBag.Menu = "Driver";
            return View();
        }

        public ActionResult GetDrivers(JQueryDataTableParamModel param)
        {
            IQueryable<Driver> drivers = applicationDbContext.Drivers.AsQueryable();
            IEnumerable<Driver> filtered;
            var driverNameSearch = Convert.ToString(Request["driverNameSearch"]);
            var DegreeSearch = Convert.ToString(Request["DegreeSearch"]);

            if (!string.IsNullOrWhiteSpace(driverNameSearch))
                drivers = drivers.Where(ivar => ivar.Name.Contains(driverNameSearch));
            if (!string.IsNullOrWhiteSpace(DegreeSearch))
                drivers = drivers.Where(ivar => ivar.Category.Contains(DegreeSearch));

            filtered = drivers.ToList();
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
                                 c.PersonalNumber,
                                 c.Name,
                                 c.Category,
                                 c.ID.ToString()
                         };
                        
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Drivers.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Driver-New")]
        public ActionResult New()
        {
            return PartialView("New", new Driver());
        }


        [HttpPost]
        [Authorize(Roles = "Driver-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(Driver model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Drivers.Add(model);
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "Driver-Edit")]
        public ActionResult Edit(int id)
        {
            var driver = applicationDbContext.Drivers.First(item => item.ID == id);

            return PartialView("Edit", driver);
        }

        [HttpPost]
        [Authorize(Roles = "Driver-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Driver model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "Driver-Delete")]
        public ActionResult Delete(int id)
        {
            var driver = applicationDbContext.Drivers.First(u => u.ID == id);
            if (driver == null)
                return HttpNotFound();
            return PartialView("Delete", driver);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Driver-Delete")]
        public ActionResult DeleteConfirmed(Driver model)
        {
            var driver = applicationDbContext.Drivers.First(u => u.ID == model.ID);
            applicationDbContext.Drivers.Remove(driver);
            applicationDbContext.SaveChanges();
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (driver == null)
            {
                return HttpNotFound();
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
           
        }
    }
}