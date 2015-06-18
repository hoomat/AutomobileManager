using AutomobilMng.Log;
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
                  var dic = LogAttribute.GetProperties<RepairModel>(null, ((int)Subject.ShowSetting).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش تنظیمات", null, dic.ToArray());
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
              
               try
               {
                   if (!string.IsNullOrWhiteSpace(Request.Files[0].FileName))
                   {
                       HttpPostedFileBase file = Request.Files["zipfile"];
                       file.SaveAs(Server.MapPath("~/Content/Images/background.jpg"));

                   }
                
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
                   var dic = LogAttribute.GetProperties<SettingModel>(model, ((int)Subject.SaveSetting).ToString(), "success");
                   Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ذخیره تنظیمات", null, dic.ToArray());
               }
               catch (System.Exception exc)
               {
                   var dicfail = LogAttribute.GetProperties<SettingModel>(model, ((int)Subject.SaveSetting).ToString(), "fail");
                   Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ذخیره تنظیمات", null, dicfail.ToArray());
                   messageModel = new MessageModel { Code = 1, Message = exc.Message };
                   return PartialView("MessageHandle", messageModel);
               }
               var dicfail0 = LogAttribute.GetProperties<SettingModel>(model, ((int)Subject.SaveSetting).ToString(), "fail");
               Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ذخیره تنظیمات", null, dicfail0.ToArray());
                messageModel = new MessageModel { Code = 0, Message = "success" };
               return PartialView("MessageHandle", messageModel);
           }
           return View("Index", model);
       }
	}
}