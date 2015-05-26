using AutomobilMng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class SettingController : BaseController
    {
       [Authorize(Roles = "Show-Setting")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.SettingMngMenu;
            ViewBag.Menu = "Setting";
            return View();
        }

       [HttpPost]
       [Authorize(Roles = "Save-Setting")]
       [ValidateAntiForgeryToken]
       public ActionResult SaveSetting(SettingModel model)
       {
           MessageModel messageModel = null;
           if (ModelState.IsValid)
           {
               if (!string.IsNullOrWhiteSpace(Request.Files[0].FileName))
               {
                   HttpPostedFileBase file = Request.Files["zipfile"];
                   file.SaveAs(Server.MapPath("~/Content/Images/background.jpg"));
                   try
                   {
                       System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                       System.Configuration.KeyValueConfigurationElement titleLogin = config.AppSettings.Settings["TitleLogin"];
                       System.Configuration.KeyValueConfigurationElement tittleTab = config.AppSettings.Settings["TittleTab"];
                       if (null != titleLogin)
                           config.AppSettings.Settings["TitleLogin"].Value = model.TitleLogin;
                       else
                           config.AppSettings.Settings.Add("TitleLogin", model.TitleLogin);
                       if (null != tittleTab)
                           config.AppSettings.Settings["TittleTab"].Value = model.TittleTab;
                       else
                           config.AppSettings.Settings.Add("TittleTab", model.TittleTab);
                       config.Save();
                   }
                   catch (System.Exception exc)
                   {
                       messageModel = new MessageModel { Code = 1, Message = exc.Message };
                       return PartialView("MessageHandle", messageModel);
                   }
               }
               else
               {
                    messageModel = new MessageModel { Code = 1, Message = (AVAResource.Resource.Chassis_Not_Unique) };
                   return PartialView("MessageHandle", messageModel);
               }
                messageModel = new MessageModel { Code = 0, Message = "success" };
               return PartialView("MessageHandle", messageModel);
           }
           return View("Index", model);
       }
	}
}