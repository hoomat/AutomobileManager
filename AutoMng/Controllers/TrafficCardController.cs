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
    public class TrafficCardController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "TrafficCard-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.TransitMngMenu;
            ViewBag.Menu = "TrafficCard";
            var dic = LogAttribute.GetProperties<RepairModel>(null, ((int)Subject.TrafficCardShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش کارت ترافیک", null, dic.ToArray());
            return View();
        }

        public ActionResult GetTrafficCards(JQueryDataTableParamModel param)
        {
            IQueryable<TrafficCard> trafficCards = applicationDbContext.TrafficCards.AsQueryable();
            IEnumerable<TrafficCard> filtered;

            var numberCard = Convert.ToString(Request["numberCard"]);
            var dateExpireFrom = Convert.ToString(Request["dateExpireFrom"]);
            var dateExpireto = Convert.ToString(Request["dateExpireto"]);
            var dateBuyFrom = Convert.ToString(Request["dateBuyFrom"]);
            var dateBuyto = Convert.ToString(Request["dateBuyto"]);
            var buyer = Convert.ToString(Request["buyer"]);

            if (!string.IsNullOrWhiteSpace(numberCard))
                trafficCards = trafficCards.Where(ivar => ivar.NumberCard.Contains(numberCard));

            if (!string.IsNullOrWhiteSpace(dateExpireFrom))
                trafficCards = trafficCards.Where(ivar => ivar.DateExpire >= DateTime.Parse(dateExpireFrom));

            if (!string.IsNullOrWhiteSpace(dateExpireto))
                trafficCards = trafficCards.Where(ivar => ivar.DateExpire <= DateTime.Parse(dateExpireto));

            if (!string.IsNullOrWhiteSpace(dateBuyFrom))
                trafficCards = trafficCards.Where(ivar => ivar.DateBuy >= DateTime.Parse(dateBuyFrom));

            if (!string.IsNullOrWhiteSpace(dateBuyto))
                trafficCards = trafficCards.Where(ivar => ivar.DateBuy <= DateTime.Parse(dateBuyto));

            if (!string.IsNullOrWhiteSpace(buyer))
                trafficCards = trafficCards.Where(ivar => ivar.Buyer.Contains(buyer));

            filtered = trafficCards.ToList();
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
            var result = from item in resultlist
                         select new[] { 
                             item.NumberCard,
                             item.TrafficCardType,
                             item.Buyer,
                             item.Cost.ToString(),
                             new PersianDateTime(item.DateBuy).ToString("yyyy/MM/dd HH:mm:ss"),  
                             new PersianDateTime(item.DateExpire).ToString("yyyy/MM/dd HH:mm:ss"),   
                             item.Description,
                          
                             item.ID.ToString()
                         };
                        
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.TrafficCards.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "TrafficCard-New")]
        public ActionResult New()
        {
            return PartialView("New", new TrafficCardModel());
        }


        [HttpPost]
        [Authorize(Roles = "TrafficCard-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(TrafficCardModel model)
        {
            if (ModelState.IsValid)
            {

                var persianDeliveryDate = PersianDateTime.Parse(model.PersianDateBuy);
                model.TrafficCard.DateBuy = persianDeliveryDate.ToDateTime();

                var persianDateExpire = PersianDateTime.Parse(model.PersianDateExpire);
                model.TrafficCard.DateExpire = persianDateExpire.ToDateTime();

                model.TrafficCard.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
                var cardtype=int.Parse(model.TrafficCardType);
                model.TrafficCard.TrafficCardType = applicationDbContext.TrafficCardTypes.FirstOrDefault(item => item.ID == cardtype).Value;
                applicationDbContext.TrafficCards.Add(model.TrafficCard);
                applicationDbContext.SaveChanges();

                var dic = LogAttribute.GetProperties<TrafficCardModel>(model, ((int)Subject.TrafficCardNew).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف کارت ترافیک", null, dic.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
    
            }
            var dicfail = LogAttribute.GetProperties<TrafficCardModel>(model, ((int)Subject.TrafficCardNew).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف کارت ترافیک", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "TrafficCard-Edit")]
        public ActionResult Edit(int id)
        {
            var trafficcard = applicationDbContext.TrafficCards.First(item => item.ID == id);
            var model = new TrafficCardModel(trafficcard);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "TrafficCard-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrafficCardModel model)
        {
            if (ModelState.IsValid)
            {
                var persianDeliveryDate = PersianDateTime.Parse(model.PersianDateBuy);
                model.TrafficCard.DateBuy = persianDeliveryDate.ToDateTime();

                var persianDateExpire = PersianDateTime.Parse(model.PersianDateExpire);
                model.TrafficCard.DateExpire = persianDateExpire.ToDateTime();
                var cardtype = int.Parse(model.TrafficCardType);
                model.TrafficCard.TrafficCardType = applicationDbContext.TrafficCardTypes.FirstOrDefault(item => item.ID == cardtype).Value;
                applicationDbContext.Entry(model.TrafficCard).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                var dic = LogAttribute.GetProperties<TrafficCardModel>(model, ((int)Subject.TrafficCardEdit).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی کارت ترافیک", null, dic.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            var dicfail = LogAttribute.GetProperties<TrafficCardModel>(model, ((int)Subject.TrafficCardEdit).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی کارت ترافیک", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "TrafficCard-Delete")]
        public ActionResult Delete(int id)
        {
            var trafficcard = applicationDbContext.TrafficCards.First(u => u.ID == id);
            var model = new TrafficCardModel(trafficcard);
            if (trafficcard == null)
                return HttpNotFound();
            return PartialView("Delete", model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TrafficCard-Delete")]
        public ActionResult DeleteConfirmed(TrafficCardModel model)
        {
            var trafficcard = applicationDbContext.TrafficCards.First(u => u.ID == model.TrafficCard.ID);
            applicationDbContext.TrafficCards.Remove(trafficcard);
            applicationDbContext.SaveChanges();
            var dic = LogAttribute.GetProperties<TrafficCardModel>(model, ((int)Subject.TrafficCardDelete).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف کارت ترافیک", null, dic.ToArray());
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (trafficcard == null)
            {
                return HttpNotFound();
            }
            var dicfail = LogAttribute.GetProperties<TrafficCardModel>(model, ((int)Subject.TrafficCardDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف کارت ترافیک", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
           
        }
    }
}