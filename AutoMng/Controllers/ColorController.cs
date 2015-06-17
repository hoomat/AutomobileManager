using AutomobilMng.Log;
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
    public class ColorController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Color-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.Menu_Color;
            ViewBag.Menu = "Color";
            var dic = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش رنگ خودرو ", null, dic.ToArray());
            return View();
        }

        [Authorize(Roles = "Color-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }
        public ActionResult GetColors(JQueryDataTableParamModel param)
        {
            IQueryable<Color> colors = applicationDbContext.Colors.AsQueryable();
            IEnumerable<Color> filtered;
            var name = Convert.ToString(Request["name"]);

            if (!string.IsNullOrWhiteSpace(name))
                colors = colors.Where(ivar => ivar.Name.Contains(name));
           
            filtered = colors.ToList();
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
                             c.Name,
                             c.Value,
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

        [Authorize(Roles = "Color-New")]
        public ActionResult New()
        {
            return PartialView("New", new Color());
        }

        [HttpPost]
        [Authorize(Roles = "Color-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(Color model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Colors.Add(model);
                applicationDbContext.SaveChanges();
                var dic = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorNew).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف رنگ خودرو ", null, dic.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            var dicfail = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorNew).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف رنگ خودرو ", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "Color-Edit")]
        public ActionResult Edit(int id)
        {
            var driver = applicationDbContext.Colors.First(item => item.ID == id);
            return PartialView("Edit", driver);
        }

        [HttpPost]
        [Authorize(Roles = "Color-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Color model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                var dic = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorEdit).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی رنگ خودرو ", null, dic.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            var dicfail = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorEdit).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی رنگ خودرو ", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "Color-Delete")]
        public ActionResult Delete(int id)
        {
            var color = applicationDbContext.Colors.First(u => u.ID == id);
            if (color == null)
                return HttpNotFound();
            return PartialView("Delete", color);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Color-Delete")]
        public ActionResult DeleteConfirmed(AutomobileClass model)
        {
            var color = applicationDbContext.Colors.First(u => u.ID == model.ID);
            applicationDbContext.Colors.Remove(color);
            applicationDbContext.SaveChanges();
            var dic = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorDelete).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف رنگ خودرو ", null, dic.ToArray());
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (color == null)
            {
                return HttpNotFound();
            }
            var dicfail = LogAttribute.GetProperties<Color>(null, ((int)Subject.ColorDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف رنگ خودرو ", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }
    }
}