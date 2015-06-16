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
    public class AutomobileClassController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "AutomobileClass-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.Menu_AutomobileClass;
            ViewBag.Menu = "AutomobileClass";
            return View();
        }

        [Authorize(Roles = "AutomobileClass-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }
        public ActionResult GetAutomobileClasses(JQueryDataTableParamModel param)
        {
            IQueryable<AutomobileClass> automobileClasses = applicationDbContext.AutomobileClasses.AsQueryable();
            IEnumerable<AutomobileClass> filtered;
            var model = Convert.ToString(Request["model"]);

            if (!string.IsNullOrWhiteSpace(model))
                automobileClasses = automobileClasses.Where(ivar => ivar.Class.Contains(model));
           
            filtered = automobileClasses.ToList();
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
                             c.Class,
                             c.ID.ToString()
                         };
                        
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Departments.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "AutomobileClass-New")]
        public ActionResult New()
        {
            return PartialView("New", new AutomobileClass());
        }

        [HttpPost]
        [Authorize(Roles = "AutomobileClass-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(AutomobileClass model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.AutomobileClasses.Add(model);
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "AutomobileClass-Edit")]
        public ActionResult Edit(int id)
        {
            var driver = applicationDbContext.AutomobileClasses.First(item => item.ID == id);

            return PartialView("Edit", driver);
        }

        [HttpPost]
        [Authorize(Roles = "AutomobileClass-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AutomobileClass model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "AutomobileClass-Delete")]
        public ActionResult Delete(int id)
        {
            var automobileModel = applicationDbContext.AutomobileClasses.First(u => u.ID == id);
            if (automobileModel == null)
                return HttpNotFound();
            return PartialView("Delete", automobileModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AutomobileClass-Delete")]
        public ActionResult DeleteConfirmed(AutomobileClass model)
        {
            var automobileModel = applicationDbContext.AutomobileClasses.First(u => u.ID == model.ID);
            applicationDbContext.AutomobileClasses.Remove(automobileModel);
            applicationDbContext.SaveChanges();
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (automobileModel == null)
            {
                return HttpNotFound();
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
           
        }
    }
}