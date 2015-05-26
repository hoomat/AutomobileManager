using DAL;
using MD.PersianDateTime;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class ReportController : BaseController
    {
        //
        // GET: /Report/
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        public ActionResult Simple()
        {
            return View();
        }
        public ActionResult FromLoadFileReport()
        {

            StiReport report = new StiReport();
            string Path = Server.MapPath("~" + ("//Reports//Automobile.mrt"));
            report.Load(Path);
            report.Compile();
         //   report["Automobile"] = applicationDbContext.Automobils.();
            report.Dictionary.Clear();
            report["CurrentUser"] = User.Identity.Name;
            report["CurrentDt"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            var aut=applicationDbContext.Automobils.ToList();
            report.RegBusinessObject("Data",aut );
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