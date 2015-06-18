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
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class RepairController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Repair-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.RepairMngMenu;
            ViewBag.Menu = "Repair";
            var dic = LogAttribute.GetProperties<RepairModel>(null, ((int)Subject.RepairShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش تعمیرات", null, dic.ToArray());
            return View();
        }

        [Authorize(Roles = "Repair-Show")]
        public ActionResult ShowRepairs(int automobileid)
        {
            var automobile = applicationDbContext.Automobiles.FirstOrDefault(item => item.ID == automobileid);
            return PartialView("ShowRepairs", automobile);
        }

        public ActionResult GetRepairs(JQueryDataTableParamModel param)
        {
            IQueryable<Repair> repairs = applicationDbContext.Repairs.AsQueryable();
            IEnumerable<Repair> filtered;

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                repairs = repairs.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

            var automobile = Convert.ToString(Request["automobile"]);
            var driver = Convert.ToString(Request["driver"]);
            var department = Convert.ToString(Request["commander"]);
            var workshop = Convert.ToString(Request["workshop"]);

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
            if (!string.IsNullOrWhiteSpace(department))
            {
                var departmentid = int.Parse(department);
                repairs = repairs.Where(ivar => ivar.DepartmentId == departmentid);
            }


                
            if (!string.IsNullOrWhiteSpace(workshop))
                repairs = repairs.Where(ivar => ivar.Workshop.Contains(workshop));

            filtered = repairs.ToList();
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
                             c.Department==null ? "" :c.Department.Name,
                             new PersianDateTime(c.DateRepair).ToString("yyyy/MM/dd HH:mm:ss"), 
                             c.Workshop,
                             c.InvoiceNo,
                                c.Wage==null ? "" :c.Wage.Value.ToString(),
                             c.Cost==null ? "" :c.Cost.Value.ToString(),
                              
                             c.RepairmanDescription,
                             c.ActionDescription,
                             c.Description,
                             c.ID.ToString()
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Repairs.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Repair-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [Authorize(Roles = "Repair-Show")]
        public ActionResult IncidentRepairs(int id)
        {
            return PartialView("IncidentRepairs",applicationDbContext.Incidents.FirstOrDefault(item=>item.ID==id));
        }

        public ActionResult GetIncidentRepairs(JQueryDataTableParamModel param)
        {
            IQueryable<Repair> repairs = applicationDbContext.Repairs.AsQueryable();
            IEnumerable<Repair> filtered;

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                repairs = repairs.Where(ivar => ivar.Automobile.DepartmentId == identityUser.DepartmentId);

           // var automobile = Convert.ToString(Request["id"]);
            var incident = Convert.ToString(Request["incident"]);
            //var commander = Convert.ToString(Request["commander"]);
            //var workshop = Convert.ToString(Request["workshop"]);

            if (!string.IsNullOrWhiteSpace(incident) && incident != (-1).ToString())
            {
                var incidentid = int.Parse(incident);
                repairs = repairs.Where(ivar => ivar.IncidentID == incidentid);
            }


            filtered = repairs.ToList();
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
                             c.Department.Name,
                            new PersianDateTime(c.DateRepair).ToString("yyyy/MM/dd HH:mm:ss"), 
                             c.Workshop,
                             c.InvoiceNo,
                                   c.Cost==null ? "" :c.Cost.Value.ToString(),
                                 c.Wage==null ? "" :c.Wage.Value.ToString(),
                             c.RepairmanDescription,
                             c.ActionDescription,
                             c.Description,
                             c.IdentityUser.UserName,
                             c.ID.ToString()
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Repairs.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsumablePartEntryEditor()
        {
            return PartialView("ConsumablePartEntryEditor", new ConsumablePartModel());
        }

        [Authorize(Roles = "Repair-New")]
        public ActionResult New()
        {
            return PartialView("New", new RepairModel(this));
        }

        [Authorize(Roles = "Repair-New")]
        public ActionResult NewIncidentRepair(int id)
        {
            return PartialView("New", new RepairModel(this,id));
        } 

        [HttpPost]
        [Authorize(Roles = "Repair-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(RepairModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Define(User))
                {
                    var dic = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairNew).ToString(), "success");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف تعمیرات", null, dic.ToArray());
                    var messageModel = new MessageModel { Code = 0, Message = "success" };
                    return PartialView("MessageHandle", messageModel);
                }
                else
                {
                    var dicfail = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairNew).ToString(), "fail");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف تعمیرات", null, dicfail.ToArray());
                    var messageModel = new MessageModel { Code = 1, Message = (AVAResource.Resource.Chassis_Not_Unique) };
                    return PartialView("MessageHandle", messageModel);
                }
            }
            var dicfail0 = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairNew).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف تعمیرات", null, dicfail0.ToArray());
            return PartialView("New", model);
            
        }

        public ActionResult Search()
        {
            return PartialView("Search",new RepairSearchModel(this));
        }

        [Authorize(Roles = "Repair-Edit")]
        public ActionResult Edit(int id)
        {
            var repair = applicationDbContext.Repairs.First(item => item.ID == id);
            var model = new RepairModel(repair,this);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Repair-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RepairModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Modify())
                {
                    var dic = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairEdit).ToString(), "success");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی تعمیرات", null, dic.ToArray());
                    var messageModel = new MessageModel { Code = 0, Message = "success" };
                    return PartialView("MessageHandle", messageModel);
                }
                else
                {
                    var dicfail = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairEdit).ToString(), "fail");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی تعمیرات", null, dicfail.ToArray());
                    var messageModel = new MessageModel { Code = 1, Message = (AVAResource.Resource.Chassis_Not_Unique) };
                    return PartialView("MessageHandle", messageModel);
                }

                var dicfail0 = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairEdit).ToString(), "fail");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی تعمیرات", null, dicfail0.ToArray());
                return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
            }
            return PartialView("Edit", model);
        }

        [Authorize(Roles = "Repair-Delete")]
        public ActionResult Delete(int id)
        {
            var repair = applicationDbContext.Repairs.First(u => u.ID == id);
            var model = new RepairModel(repair,this);
            if (repair == null)
                return HttpNotFound();
            return PartialView("Delete", model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Repair-Delete")]
        public ActionResult DeleteConfirmed(RepairModel model)
        {
            if (model.Delete())
            {
                var dic = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairDelete).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف تعمیرات", null, dic.ToArray());
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            var dicfail = LogAttribute.GetProperties<RepairModel>(model, ((int)Subject.RepairDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف تعمیرات", null, dicfail.ToArray());
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [AllowAnonymous]
        public ActionResult ChartRepairAutomobile()
        {
            string[] categories = applicationDbContext.Automobiles.Select(item => item.Plaque).ToArray();
            List<Series> Series = new List<Series>();
            List<object> listdata = new List<object>();

            foreach (var category in categories.ToList())
            {
                listdata.Add(applicationDbContext.Repairs.Count(item => item.Automobile.Plaque == category));
            }
            Series.Add(new Series { Name = "خودروها", Data = new Data(listdata.ToArray()) });
            Highcharts chart = new Highcharts("chart") { }

                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                  .SetTitle(new Title { Text = "نمودار تعداد تعمیرات خودرو " })
                  .SetXAxis(new XAxis { Categories = categories .ToArray()})
                  .SetYAxis(new YAxis
                  {
                      AllowDecimals=false,
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
                      // Floating = true,
                      BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                      Shadow = true,
                      
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
            var dic = LogAttribute.GetProperties<RepairModel>(null, ((int)Subject.RepairChartAutomobile ).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش نمودار تعداد تعمیرات خودرو", null, dic.ToArray());

            return PartialView(chart);
        }

        [AllowAnonymous]
        public ActionResult RepairWorkshopReferral()
        {
           // string[] categories = applicationDbContext.Automobiles.Select(item => item.Plaque).ToArray();
            List<Series> Series = new List<Series>();
            List<object> listdata = new List<object>();
            List<WorkshopReferralModel> filtered =
              (from row in applicationDbContext.Repairs
               group row by new { row.Workshop } into grouped
               select new WorkshopReferralModel()
               {
                   Workshop = grouped.Key.Workshop,
                   Referral = grouped.Count()
               }).ToList();

            foreach (var category in filtered)
                listdata.Add(category.Referral);

          

            Series.Add(new Series { Name = "آمار مراجعه", Data = new Data(listdata.ToArray()) });
            Highcharts chart = new Highcharts("chart") { }

                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                  .SetTitle(new Title { Text = "نمودار آمار مراجعه به تعمیرگاه" })
                  .SetXAxis(new XAxis { Categories = filtered.Select(item=>item.Workshop).ToArray() })
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

            var dic = LogAttribute.GetProperties<RepairModel>(null, ((int)Subject.RepairChartWorkshopReferral).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش نمودار مراجعه به تعمیرگاه", null, dic.ToArray());
            return PartialView(chart);
        }



        [Authorize(Roles = "Transit-Report")]
        public ActionResult Report(string automobile, string driver, string commander, string workshop)
        {
            ViewBag.automobile = automobile;
            ViewBag.driver = driver;
            ViewBag.commander = commander;
            ViewBag.workshop = workshop;
            var dic = LogAttribute.GetProperties<RepairModel>(null, ((int)Subject.RepairChartWorkshopReferral).ToString(), "success");
            dic.Add(new KeyValuePair<string, string>("automobile", automobile == null ? "" : automobile));
            dic.Add(new KeyValuePair<string, string>("driver", driver == null ? "" : driver));
            dic.Add(new KeyValuePair<string, string>("commander", commander == null ? "" : commander));
            dic.Add(new KeyValuePair<string, string>("workshop", workshop == null ? "" : workshop));
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش گزارش", null, dic.ToArray());
            return PartialView("Report");
        }
        public ActionResult FromLoadFileReport(string automobile, string driver, string commander, string workshop)
        {
            IQueryable<Repair> repairs = applicationDbContext.Repairs.AsQueryable();

            var identityUser = applicationDbContext.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if (identityUser.GroupId == (int)GroupModel.User || identityUser.GroupId == (int)GroupModel.StuckReport)
                repairs = repairs.Where(ivar => ivar.Automobile.DepartmentId== identityUser.DepartmentId);

            IEnumerable<Repair> filtered;

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
            if (!string.IsNullOrWhiteSpace(commander))
            {
                var departmentid = int.Parse(commander);
                repairs = repairs.Where(ivar => ivar.DepartmentId == departmentid);
            }
            
            if (!string.IsNullOrWhiteSpace(workshop))
                repairs = repairs.Where(ivar => ivar.Workshop.Contains(workshop));
            
            filtered = repairs.ToList();


            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//Repair.mrt"));
            report.Load(Path);
            report.Compile();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut = filtered;
            report.RegBusinessObject("Data", filtered.Select(item => new RepiarReportModel
            {
                Automobile = item.Automobile.Plaque,
                Commander = item.Department.Name,
                Cost =(item.Cost==null ? "" :item.Cost.Value.ToString()),
                DateRepair = new PersianDateTime(item.DateRepair).ToString("yyyy/MM/dd"),
                Driver = item.Driver.Name,
                Workshop = item.Workshop
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