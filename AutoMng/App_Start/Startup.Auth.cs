using AutomobilMng.Log;
using AutomobilMng.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Collections.Generic;
using System.Web.Hosting;

namespace AutomobilMng
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
//            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
//var result=UserManager.CreateAsync(new ApplicationUser {  LastName="", FirstName="", UserName="1", PasswordHash="123456", },"123456");
//            ApplicationDbContext db = new ApplicationDbContext();
//            db.Database.CreateIfNotExists();

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

            System.Configuration.KeyValueConfigurationElement titleLogin = config.AppSettings.Settings["TitleLogin"];
            System.Configuration.KeyValueConfigurationElement tittleTab = config.AppSettings.Settings["TittleTab"];
            if (null != titleLogin)
            {
                // lets do something with the value - displaying it in a textbox is an ides
                Config.TitleLogin = titleLogin.Value;
            }
            if (null != tittleTab)
            {
                // lets do something with the value - displaying it in a textbox is an ides
                Config.TittleTab = tittleTab.Value;
            }
           var path=  HostingEnvironment.MapPath(@"~/Log/log4netconfig.xml");
           Logger.Initialize(new System.Uri(path));

           Logger.Send(GetType(), Logger.CriticalityLevel.Info,"" ,"statistical",null,
                                    new[]{ new KeyValuePair<string, string>("value",""),
                                                     new KeyValuePair<string, string>("analyzerid", "")});
            var type = typeof(DatabaseAppender);
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, "", "statistical", null,
                                        new[]{ new KeyValuePair<string, string>("value",""),
                                                     new KeyValuePair<string, string>("analyzerid", "")});
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}