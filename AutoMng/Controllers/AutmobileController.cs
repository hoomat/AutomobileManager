using AutomobilMng.Log;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Point = DotNet.Highcharts.Options.Point;

namespace AutomobilMng.Controllers
{
    public class AutmobileController :BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Automobile-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.AutomobileMngMenu;
            ViewBag.Menu = "Automobile";
            var dic = LogAttribute.GetProperties<ApplicationUser>(null, ((int)Subject.AutomobileShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش خودروها", null, dic.ToArray());
            int BlockSize = 10;
            var autos = GetAutomobilsPaging(1, BlockSize);
            return View(autos);
        }

        public ActionResult GetAutomobiles(JQueryDataTableParamModel param)
        {
            IQueryable<Automobile> automobils = applicationDbContext.Automobiles.AsQueryable();
            IEnumerable<Automobile> filtered;
            var plaqueSearch = Convert.ToString(Request["plaqueSearch"]);
            var chassisSearch = Convert.ToString(Request["chassisSearch"]);
            var modelSearch = Convert.ToString(Request["modelSearch"]);
            var produceYear = Convert.ToString(Request["produceYearSearch"]);
            var fualTypeSearch = Convert.ToString(Request["fualTypeSearch"]);
            var departmentSearch = Convert.ToString(Request["departmentSearch"]);

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                automobils = automobils.Where(ivar => ivar.DepartmentId == identityUser.DepartmentId);

            if (!string.IsNullOrWhiteSpace(plaqueSearch))
                automobils = automobils.Where(ivar => ivar.Plaque.Contains(plaqueSearch));
            if (!string.IsNullOrWhiteSpace(chassisSearch))
                automobils = automobils.Where(ivar => ivar.Chassis.Contains(chassisSearch));
            //if (!string.IsNullOrWhiteSpace(modelSearch))
            //    automobils = automobils.Where(ivar => ivar.Model == modelSearch);

            if (!string.IsNullOrWhiteSpace(modelSearch) && modelSearch != (-1).ToString())
            {
                var modelSearchid = int.Parse(modelSearch);
                var automobileClass = applicationDbContext.AutomobileClasses.FirstOrDefault(item => item.ID == modelSearchid);
                automobils = automobils.Where(ivar => ivar.AutomobileClassId == automobileClass.ID);
            }


            if (!string.IsNullOrWhiteSpace(produceYear))
                automobils = automobils.Where(ivar => ivar.ProduceYear == produceYear);
            if (!string.IsNullOrWhiteSpace(fualTypeSearch) && fualTypeSearch != (-1).ToString())
            {
                var fualTypeSearchid = int.Parse(fualTypeSearch);
                var fueltype = applicationDbContext.FualTypes.FirstOrDefault(item => item.ID == fualTypeSearchid);
                automobils = automobils.Where(ivar => ivar.FualType == fueltype.Value);
            }
            if (!string.IsNullOrWhiteSpace(departmentSearch) && departmentSearch != (-1).ToString())
            {
                var departmentSearchid = int.Parse(departmentSearch);
                var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentSearchid);
                automobils = automobils.Where(ivar => ivar.Department.Name == department.Name);

            }
            filtered = automobils.ToList();
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
                             c.Plaque,
                             c.Chassis,
                            c.AutomobileClass==null?"":c.AutomobileClass.Class,
                              c.ProduceYear,
                               c.FualType,
                             new PersianDateTime(c.DateBuy).ToString("yyyy/MM/dd"),
                             c.Color==null?"":c.Color.Value,
                             c.Department.Name,
                             c.AutomobileStatus==null?"":c.AutomobileStatus.Status,
                             c.ID.ToString(),
                             c.ID.ToString()
               
            };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Automobiles.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Automobile-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [Authorize(Roles = "Automobile-New")]
        public ActionResult New()
        {
            return PartialView("New", new AutomobileModel());
        }

        [HttpPost]
        [Authorize(Roles = "Automobile-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(AutomobileModel model)
        {
            if (ModelState.IsValid)
            {
                
                var persianDateTime = PersianDateTime.Parse(model.PersianDateBuy);
                model.Automobile.DateBuy = persianDateTime.ToDateTime();
                var search = applicationDbContext.Automobiles.FirstOrDefault(item => item.Chassis == model.Automobile.Chassis);
                if (search == null)
                {
                    model.Automobile.AutomobileStatusId = 1;
                    model.Automobile.LastService = DateTime.Now;
                    applicationDbContext.Automobiles.Add(model.Automobile);
                    model.Automobile.IdentityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
                  //  model.Automobile.AutomobileDrivers = new List<AutomobileDriver>();
                    //if (model.SelectedDrivers != null)
                    //{
                    //    foreach (var select in model.SelectedDrivers)
                    //    {
                    //        int driverid = int.Parse(select.ToString());
                    //        var driver = applicationDbContext.Drivers.FirstOrDefault(item => item.ID == driverid);
                    //        if (driver != null)
                    //            model.Automobile.AutomobileDrivers.Add(new AutomobileDriver { Automobile = model.Automobile, Driver = driver });
                    //    }
                    //}
                    if (!string.IsNullOrWhiteSpace(model.DepartmentID))
                    {
                        int departmentID = int.Parse(model.DepartmentID.ToString());
                        var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentID);
                        model.Automobile.Department = department;
                    }
                    if (!string.IsNullOrWhiteSpace(model.Automobile.FualType))
                    {
                        int fualTypeid = int.Parse(model.Automobile.FualType);
                        var fualType = applicationDbContext.FualTypes.FirstOrDefault(item => item.ID == fualTypeid);
                        model.Automobile.FualType= fualType.Value;
                    }
                    if (!string.IsNullOrWhiteSpace(model.ClasssId))
                    {
                        int classid = int.Parse(model.ClasssId);
                      

                        model.Automobile.AutomobileClassId = classid;
                    }
                    if (!string.IsNullOrWhiteSpace(model.ColorId))
                    {
                        int colorId = int.Parse(model.ColorId);

                        model.Automobile.ColorId = colorId;
                    }
                    applicationDbContext.SaveChanges();
                    if (!string .IsNullOrWhiteSpace(Request.Files[0].FileName))
                    {
                        HttpPostedFileBase file = Request.Files["zipfile"];
                        file.SaveAs(Server.MapPath("~/AutomobilImages/" + model.Automobile.ID+"."+file.FileName.Split(new[]{'.'},StringSplitOptions.RemoveEmptyEntries)[1]));
                        model.Automobile.ImageAddress = (model.Automobile.ID + "." + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        applicationDbContext.SaveChanges();
                    }
                    var dic = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileNew).ToString(), "success");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف خودرو", null, dic.ToArray());
                    var messageModel = new MessageModel { Code = 0, Message = "success" };
                    return PartialView("MessageHandle", messageModel);
                }
                else
                {
                    var dic = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileNew).ToString(), "fail");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف خودرو", null, dic.ToArray());
                    var messageModel = new MessageModel { Code = 1, Message = (AVAResource.Resource.Chassis_Not_Unique) };
                    return PartialView("MessageHandle", messageModel);
                }
            }

            return PartialView("New", model);
        }

        public ActionResult Search()
        {
            return PartialView("Search", new AutomobileSearchModel());
        }






        [Authorize(Roles = "Automobile-ChangeStatus")]
        public ActionResult ChangeStatus(int id)
        {
            var automobile = applicationDbContext.Automobiles.First(item => item.ID== id);
            var model = new AutomobileModel(automobile);
            return PartialView("ChangeStatus", model);
        }

        [HttpPost]
        [Authorize(Roles = "Automobile-ChangeStatus")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(AutomobileModel model)
        {
            if (ModelState.IsValid)
            {
              var auto=  applicationDbContext.Automobiles.FirstOrDefault(item => item.ID == model.Automobile.ID);

                if (!string.IsNullOrWhiteSpace(model.StatusId))
                {
                    int automobileStatusId = int.Parse(model.StatusId);
                    auto.AutomobileStatusId = automobileStatusId;
                }
                 applicationDbContext.SaveChanges();
                 var dic = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileChangeStatus).ToString(), "success");
                 Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تغییر وضعیت خودرو", null, dic.ToArray());
                var messageModel = new MessageModel { Code = 0, Message = "success" };
                return PartialView("MessageHandle", messageModel);
            }
            var dicfail = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileChangeStatus).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تغییر وضعیت خودرو", null, dicfail.ToArray());
            return PartialView("ChangeStatus", model);
        }







        [Authorize(Roles = "Automobile-Edit")]
        public ActionResult Edit(int id)
        {
            var automobile = applicationDbContext.Automobiles.First(item => item.ID == id);
            var model = new AutomobileModel(automobile);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Automobile-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AutomobileModel model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Automobiles.Attach(model.Automobile);
                var persianDateTime = PersianDateTime.Parse(model.PersianDateBuy);
                model.Automobile.DateBuy = persianDateTime.ToDateTime();
                if (!string.IsNullOrWhiteSpace(Request.Files[0].FileName))
                {
                    HttpPostedFileBase file = Request.Files["zipfile"];
                    file.SaveAs(Server.MapPath("~/AutomobilImages/" + model.Automobile.ID + "." + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1]));
                    model.Automobile.ImageAddress = (model.Automobile.ID + "." + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1]);

                }
                if (!string.IsNullOrWhiteSpace(model.DepartmentID))
                {
                    int departmentID = int.Parse(model.DepartmentID.ToString());
                    var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentID);
                    model.Automobile.Department = department;
                }

                if (!string.IsNullOrWhiteSpace(model.Automobile.FualType))
                {
                    int fualTypeid = int.Parse(model.Automobile.FualType);
                    var fualType = applicationDbContext.FualTypes.FirstOrDefault(item => item.ID == fualTypeid);
                    model.Automobile.FualType = fualType.Value;
                }
                if (!string.IsNullOrWhiteSpace(model.ClasssId))
                {
                    int classid = int.Parse(model.ClasssId);


                    model.Automobile.AutomobileClassId = classid;
                }
                if (!string.IsNullOrWhiteSpace(model.ColorId))
                {
                    int colorId = int.Parse(model.ColorId);

                    model.Automobile.ColorId = colorId;
                }
                applicationDbContext.Entry(model.Automobile).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                var dic = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileEdit).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی خودرو", null, dic.ToArray());
                var messageModel = new MessageModel { Code = 0, Message = "success" };
                return PartialView("MessageHandle", messageModel);
            }
            var dicfail = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileEdit).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی خودرو", null, dicfail.ToArray());
            return PartialView("Edit", model);
        }




        [Authorize(Roles = "Automobile-Delete")]
        public ActionResult Delete(int id )
        {
            var automobil = applicationDbContext.Automobiles.First(u => u.ID == id);
            var model = new AutomobileModel(automobil);
            if (automobil == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", model);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Automobile-Delete")]
        public ActionResult DeleteConfirmed(AutomobileModel model)
        {
            var automobile = applicationDbContext.Automobiles.First(u => u.ID == model.Automobile.ID);
            applicationDbContext.Automobiles.Remove(automobile);
            applicationDbContext.SaveChanges();
            var dic = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileDelete).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف خودرو", null, dic.ToArray());
            var messageModel = new MessageModel { Code = 0, Message = "success" };
            return PartialView("MessageHandle", messageModel);
            if (automobile == null)
            {
                return HttpNotFound();
            }
            var dicfail = LogAttribute.GetProperties<AutomobileModel>(model, ((int)Subject.AutomobileDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف خودرو", null, dicfail.ToArray());
            return PartialView("Delete", model);
            
        }

        public ActionResult GetAutomobilList(List<AutomobileModel> automobiles)
        {
            return PartialView(automobiles);
        }

        [AllowAnonymous]
        public ActionResult GetDrivers(int automobileid)
        {
            //var states = null;//applicationDbContext.AutomobileDrivers.Where(item => item.Automobile.ID == automobileid);
            //return Json(new SelectList(
            //                states.ToArray(),
            //                "Driver.ID",
            //                "Driver.Name")
            //           , JsonRequestBehavior.AllowGet);
            return null;
        }
        [HttpPost]
        public ActionResult InfinateScroll(int BlockNumber)
        {
            int BlockSize = 10;
            var automobiles = GetAutomobilsPaging(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = automobiles.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("GetAutomobilList", automobiles);
            return Json(jsonModel);
        }

        protected string RenderPartialViewToString(string view, object model)
        {
            if (string.IsNullOrEmpty(view))
                view = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, view);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public  List<AutomobileModel> GetAutomobilsPaging(int BlockNumber, int BlockSize)
        {
            int startIndex = (BlockNumber - 1) * BlockSize;
            var automobiles = applicationDbContext.Automobiles.OrderByDescending(item=>item.ID).Skip(startIndex).Take(BlockSize).ToList();
            List<AutomobileModel> models = new List<AutomobileModel>();
            foreach (var auto in automobiles)
            {
                var automobileModel=new  AutomobileModel();
                automobileModel.Automobile=auto;
                automobileModel.PersianDateBuy=new PersianDateTime(auto.DateBuy).ToString("yyyy/MM/dd");
                automobileModel.PersianLastService=new PersianDateTime(auto.LastService).ToString("yyyy/MM/dd HH:mm:ss");
                models.Add(automobileModel);
            }
            return models;
        }

        [Authorize(Roles = "Automobile-Statistics-Analysis")]
        public ActionResult ChartDeliverAutomobile()
        {
            string[] categories = applicationDbContext.Departments.Select(item => item.Name).ToArray();
            List<Series> Series = new List<Series>();
            List<object> listdata = new List<object>();
           
            foreach (var category in categories.ToList())
            {
                listdata.Add(applicationDbContext.Automobiles.Count(item => item.Department.Name == category));
            }
            Series.Add(new Series { Name = "تحویل دایمی", Data = new Data(listdata.ToArray()) });
            Highcharts chart = new Highcharts("chart") { }
            
                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                  .SetTitle(new Title { Text = "نمودار تحویل خودرو به ادارات" })
                  .SetXAxis(new XAxis { Categories = applicationDbContext.Departments.Select(item => item.Name).ToArray() })
                  .SetYAxis(new YAxis
                  {
                      AllowDecimals=false,
                      Min = 0,
                      Title = new YAxisTitle { Text = "بر اساس تعداد" }
                  })
                  .SetLegend(new Legend
                  {
                      
                      Rtl=true,
                      Layout = Layouts.Vertical,
                      Align = HorizontalAligns.Left,
                      VerticalAlign = VerticalAligns.Top,
                      X = 100,
                      Y = 70,
                     // Floating = true,
                      BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                      Shadow = true
                  })
                  .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.x +': '+ this.y ; }" })
                  .SetPlotOptions(new PlotOptions
                  {
                      Column = new PlotOptionsColumn
                      {
                          PointPadding = 0.2,
                          BorderWidth = 0
                      }
                  })
                  .SetSeries(
                  Series.ToArray()
                //  new[]
                //{
                //    new Series { Name = "تحویل دایمی", Data = new Data(new object[] { 49.9, 71.5 }) },
                //   // new Series { Name = "تحویل موقت", Data = new Data(new object[] { 48.9, 38.8}) },
                //}
                );

            var dic = LogAttribute.GetProperties<AutomobileModel>(null, ((int)Subject.AutomobileStatisticsAnalysis).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, " تحلیل آماری خودروها", null, dic.ToArray());
            return PartialView(chart);
        }


        [Authorize(Roles = "Automobile-Statistics-Analysis")]
        public ActionResult StatisticsAnalysis()
        {
            ViewBag.MenuShow = AVAResource.Resource.AutomobileStatisticsAnalysis;
            ViewBag.Menu = "AutomobileStatisticsAnalysis";
            return View();
        }

        
        [Authorize(Roles = "Automobile-Statistics-Analysis")]
        public ActionResult ChartCompareFuelDistance()
        {
            List<Series> Series = new List<Series>();
        //    List<object> listdatadistance = new List<object>();
            List<object> listvolume = new List<object>();

            List<FuelConsumeChartModel> filtered =
              (from row in applicationDbContext.FuelConsumes
               group row by new { row.Automobile.Plaque } into grouped
               select new FuelConsumeChartModel()
               {
                   Automobile = grouped.Key.Plaque,
                   Volume = (grouped.Sum(x => x.VolumeFuel) * 100) / grouped.Sum(x => x.Distance)
               }).ToList();

            foreach (var filter in filtered)
                listvolume.Add(filter.Volume);
            Series.Add(new Series { Name = "مصرف سوخت به مسافت ", Data = new Data(listvolume.ToArray()) });



            //List<TransitDistanceModel> filteredcost =
            //    (from row in applicationDbContext.FuelConsumes
            //     group row by new { row.Automobile.Plaque } into grouped
            //     select new TransitDistanceModel()
            //     {
            //         Automobile = grouped.Key.Plaque,
            //         Distance = grouped.Sum(x => x.Distance)
            //     }).ToList();

            //foreach (var filter in filteredcost)
            //    listdatadistance.Add(filter.Distance);
            //Series.Add(new Series { Name = "مسافت ", Data = new Data(listdatadistance.ToArray()) });



            Highcharts chart = new Highcharts("chart") { }

                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                  .SetTitle(new Title { Text = "نمودار مسافت طی شده و مصرف سوخت خودروها" })
                  .SetXAxis(new XAxis { Categories = filtered.Select(item => item.Automobile).ToArray() })
                  .SetYAxis(new YAxis
                  {
                      AllowDecimals = false,
                      Min = 0,
                      Title = new YAxisTitle { Text = "بر اساس تعداد" }
                  })
                  .SetLegend(new Legend
                  {

                      Rtl = true,
                      Layout = Layouts.Vertical,
                      Align = HorizontalAligns.Left,
                      VerticalAlign = VerticalAligns.Top,
                      X = 100,
                      Y = 70,
                       Floating = true,
                      BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                      Shadow = true
                  })
                  .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.x +': '+ this.y ; }" })
                  .SetPlotOptions(new PlotOptions
                  {
                      Column = new PlotOptionsColumn
                      {
                          PointPadding = 0.2,
                          BorderWidth = 0
                      }
                  })
                  .SetSeries(
                  Series.ToArray()
                //  new[]
                //{
                //    new Series { Name = "تحویل دایمی", Data = new Data(new object[] { 49.9, 71.5 }) },
                //   // new Series { Name = "تحویل موقت", Data = new Data(new object[] { 48.9, 38.8}) },
                //}
                );

            var dic = LogAttribute.GetProperties<AutomobileModel>(null, ((int)Subject.AutomobileStatisticsAnalysis).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, " تحلیل آماری خودروها", null, dic.ToArray());
            return PartialView(chart);
        }
        
        [HttpPost]
        public ActionResult InfinateScrollSearch(int BlockNumber, string plaqueSearch)
        {
            int BlockSize = 10;
            var automobiles = GetAutomobilsPagingSearch(BlockNumber, BlockSize, plaqueSearch);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = automobiles.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("GetAutomobilList", automobiles);
            return Json(jsonModel);
        }

        public List<AutomobileModel> GetAutomobilsPagingSearch(int BlockNumber, int BlockSize, string plaqueSearch)
        {
            IQueryable<Automobile> automobils = applicationDbContext.Automobiles.AsQueryable();
            IEnumerable<Automobile> filtered;

            if (!string.IsNullOrWhiteSpace(plaqueSearch))
                automobils = automobils.Where(ivar => ivar.Plaque.Contains(plaqueSearch));

            int startIndex = (BlockNumber - 1) * BlockSize;
             filtered = automobils.OrderByDescending(item => item.ID).Skip(startIndex).Take(BlockSize).ToList();
            List<AutomobileModel> models = new List<AutomobileModel>();
            foreach (var auto in filtered)
            {
                var automobileModel = new AutomobileModel();
                automobileModel.Automobile = auto;
                automobileModel.PersianDateBuy = new PersianDateTime(auto.DateBuy).ToString("yyyy/MM/dd");
                automobileModel.PersianLastService = new PersianDateTime(auto.LastService).ToString("yyyy/MM/dd HH:mm:ss");
                models.Add(automobileModel);
            }
            return models;
        }


        [Authorize(Roles = "Automobile-Report")]
        public ActionResult Report(string plaqueSearch, string chassisSearch, string modelSearch, string produceYear, string fualTypeSearch, string departmentSearch)
        {
            ViewBag.plaqueSearch = plaqueSearch;
            ViewBag.chassisSearch = chassisSearch;
            ViewBag.modelSearch = modelSearch;
            ViewBag.produceYear = produceYear;
            ViewBag.fualTypeSearch = fualTypeSearch;
            ViewBag.departmentSearch = departmentSearch;
            var dic = LogAttribute.GetProperties<AutomobileModel>(null, ((int)Subject.AutomobileReport).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, " گزارش خودروها", null, dic.ToArray());
            return PartialView("Report");
        }

        ////Reporter actions
        public ActionResult FromLoadFileReport(string plaqueSearch, string chassisSearch, string modelSearch, string produceYear, string fualTypeSearch, string departmentSearch)
        {
            IQueryable<Automobile> automobils = applicationDbContext.Automobiles.AsQueryable();
            IEnumerable<Automobile> filtered;
     
            if (!string.IsNullOrWhiteSpace(plaqueSearch))
                automobils = automobils.Where(ivar => ivar.Plaque.Contains(plaqueSearch));
            if (!string.IsNullOrWhiteSpace(chassisSearch))
                automobils = automobils.Where(ivar => ivar.Chassis.Contains(chassisSearch));
            if (!string.IsNullOrWhiteSpace(modelSearch) && modelSearch != (-1).ToString())
            {
                var modelSearchid = int.Parse(modelSearch);
                var automobileClass = applicationDbContext.AutomobileClasses.FirstOrDefault(item => item.ID == modelSearchid);
                automobils = automobils.Where(ivar => ivar.AutomobileClassId == automobileClass.ID);
            }

            if (!string.IsNullOrWhiteSpace(produceYear))
                automobils = automobils.Where(ivar => ivar.ProduceYear == produceYear);
            if (!string.IsNullOrWhiteSpace(fualTypeSearch) && fualTypeSearch != (-1).ToString())
            {
                var fualTypeSearchid=int.Parse(fualTypeSearch);
                var fueltype = applicationDbContext.FualTypes.FirstOrDefault(item => item.ID == fualTypeSearchid);
                automobils = automobils.Where(ivar => ivar.FualType == fueltype.Value);
            }
            if (!string.IsNullOrWhiteSpace(departmentSearch) && departmentSearch != (-1).ToString())
            {
                var departmentSearchid = int.Parse(departmentSearch);
                var department = applicationDbContext.Departments.FirstOrDefault(item => item.ID == departmentSearchid);
                automobils = automobils.Where(ivar => ivar.Department.Name == department.Name);
            }
            filtered = automobils.ToList();


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//Automobile.mrt"));
            report.Load(Path);
            report.Compile();
            //   report["Automobile"] = applicationDbContext.Automobiles.();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
            report.RegBusinessObject("Data", aut.Select(item => new AutomobileReportModel
            {
                Plaque=item.Plaque,
                Department=item.Department.Name,
                Chassis = item.Chassis,
                Color = item.Color==null?"":item.Color.Value,
                DateBuy = new PersianDateTime(item.DateBuy).ToString("yyyy/MM/dd"),
                FualConsume = item.FualConsume,
                FualType = item.FualType,
                Model = item.AutomobileClass==null?"":item.AutomobileClass.Class,
                Price = item.Price,
                ProduceYear = item.ProduceYear
            }));
            //  report.Dictionary.Synchronize();
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