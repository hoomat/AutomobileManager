using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class HomeController : BaseController
    {
         [Authorize(Roles = "Dashboard-Menu")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Menu = "Dashboard";
            return View();
        }
    }
}