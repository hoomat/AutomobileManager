using AutomobilMng.Models;
using DAL;
using MD.PersianDateTime;
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
            return View();
        }

        public ActionResult GetOilChanges(JQueryDataTableParamModel param)
        {
            IQueryable<OilChange> oilChanges = applicationDbContext.OilChanges.AsQueryable();
            IEnumerable<OilChange> filtered;
            var driverSearch = Convert.ToString(Request["driverSearch"]);
            var departmentSearch = Convert.ToString(Request["departmentSearch"]);
            var plaqueSearch = Convert.ToString(Request["plaqueSearch"]);
            if (!string.IsNullOrWhiteSpace(driverSearch))
                oilChanges = oilChanges.Where(ivar => ivar.Driver.Name.Contains(driverSearch));
            if (!string.IsNullOrWhiteSpace(departmentSearch))
                oilChanges = oilChanges.Where(ivar => ivar.Department.Name.Contains(departmentSearch));
            if (!string.IsNullOrWhiteSpace(plaqueSearch))
                oilChanges = oilChanges.Where(ivar => ivar.Automobile.Plaque == plaqueSearch);
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

            var resultlist = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Automobile.Plaque,
                             c.Driver.Name,
                             c.Department.Name,
                            new PersianDateTime(c.ChangeDate).ToString("yyyy/MM/dd"),
                             c.TypeOil,c.Workshop,
                             c.OilFilterChanged.ToString(),
                             c.AirFilterChanged.ToString(),
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
            return PartialView("New", new OilChangeModel());
        }


        [HttpPost]
        [Authorize(Roles = "OilChange-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(OilChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Parse(model.PersianChangeDate);
                model.OilChange.ChangeDate = persianDateTime.ToDateTime();

                model.OilChange.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);

                if (model.OilFilterChanged != null && model.OilFilterChanged == "on")
                    model.OilChange.OilFilterChanged = true;
                else
                    model.OilChange.OilFilterChanged = false;
                if (model.AirFilterChanged != null && model.AirFilterChanged == "on")
                    model.OilChange.AirFilterChanged = true;
                else
                    model.OilChange.AirFilterChanged = false;

                var automobileid = int.Parse(model.AutomobileID);
                model.OilChange.AutomobileID = automobileid;

                var driverid = int.Parse(model.DriverID);
                model.OilChange.DriverID = driverid;

                var departmentid = int.Parse(model.DepartmentID);
                model.OilChange.DepartmentID = departmentid;

                applicationDbContext.OilChanges.Add(model.OilChange);
                applicationDbContext.SaveChanges();
                var messageModel = new MessageModel { Code = 0, Message = "success" };
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "OilChange-Edit")]
        public ActionResult Edit(int id)
        {
            var OilChanges = applicationDbContext.OilChanges.First(item => item.ID == id);
            var model = new OilChangeModel(OilChanges);
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
                if (model.OilFilterChanged != null && model.OilFilterChanged == "on")
                    model.OilChange.OilFilterChanged = true;
                else
                    model.OilChange.OilFilterChanged = false;
                if (model.AirFilterChanged != null && model.AirFilterChanged == "on")
                    model.OilChange.AirFilterChanged = true;
                else
                    model.OilChange.OilFilterChanged = false;
                var automobileid = int.Parse(model.AutomobileID);
                model.OilChange.AutomobileID = automobileid;

                var driverid = int.Parse(model.DriverID);
                model.OilChange.DriverID = driverid;

                var departmentid = int.Parse(model.DepartmentID);
                model.OilChange.DepartmentID = departmentid;


                applicationDbContext.Entry(model.OilChange).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                var messageModel = new MessageModel { Code = 0, Message = "success" };
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "OilChange-Delete")]
        public ActionResult Delete(int id)
        {
            var OilChanges = applicationDbContext.OilChanges.First(u => u.ID == id);
            var model = new OilChangeModel(OilChanges);
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
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (OilChanges == null)
            {
                return HttpNotFound();
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
           
        }
    }
}