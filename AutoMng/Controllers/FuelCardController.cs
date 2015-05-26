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
    public class FuelCardController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "FuelCard-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.Menu_FuelCard;
            ViewBag.Menu = "FuelCard";
            return View();
        }

        public ActionResult GetFuelCards(JQueryDataTableParamModel param)
        {
            IQueryable<FuelCard> fuelCards = applicationDbContext.FuelCards.AsQueryable();
            IEnumerable<FuelCard> filtered;
            var numberSearch = Convert.ToString(Request["numberSearch"]);
   

            if (!string.IsNullOrWhiteSpace(numberSearch))
                fuelCards = fuelCards.Where(ivar => ivar.Number.Contains(numberSearch));

            filtered = fuelCards.ToList();
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
                             c.Number,
                             c.ID.ToString()
                         };
                        
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.FuelCards.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "FuelCard-New")]
        public ActionResult New()
        {
            return PartialView("New", new FuelCard());
        }


        [HttpPost]
        [Authorize(Roles = "FuelCard-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(FuelCard model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.FuelCards.Add(model);
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "FuelCard-Edit")]
        public ActionResult Edit(int id)
        {
            var fuelCard = applicationDbContext.FuelCards.First(item => item.ID == id);
            return PartialView("Edit", fuelCard);
        }

        [HttpPost]
        [Authorize(Roles = "FuelCard-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FuelCard model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "FuelCard-Delete")]
        public ActionResult Delete(int id)
        {
            var fuelCard = applicationDbContext.FuelCards.First(u => u.ID == id);
            if (fuelCard == null)
                return HttpNotFound();
            return PartialView("Delete", fuelCard);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FuelCard-Delete")]
        public ActionResult DeleteConfirmed(FuelCard model)
        {
            var driver = applicationDbContext.FuelCards.First(u => u.ID == model.ID);
            applicationDbContext.FuelCards.Remove(driver);
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