using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AutomobilMng
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes.MapRoute(
            //     name: "Default",
            //     url: "{controller}/{action}/{id}",
            //     defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            // );
            // routes.MapRoute(
            //    name: "Localization",
            //    url: "{lang}/{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            //);

            // routes.MapRoute(
            //     "Default",
            //     "{controller}/{action}/{id}", // URL with parameters
            //     new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //     );

            // routes.MapRoute(
            //"Localization", // Route name
            //"{lang}/{controller}/{action}/{id}", // URL with parameters
            //new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);


            //routes.MapHttpRoute(
            //               name: "DefaultApi",
            //               routeTemplate: "api/{controller}/{id}",
            //               defaults: new { id = RouteParameter.Optional }
            //               );

            routes.MapRoute(
                "LogOn", // Route name
                "Account/{action}", // URL with parameters
                new {controller = "Account", action = "Login"} // Parameter defaults
                );

            routes.MapRoute(
                "Autmobileid", // Route name
                "Autmobile/{action}/{id}", // URL with parameters
                new {controller = "Autmobile", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "Autmobile", // Route name
                "Autmobile/{action}", // URL with parameters
                new {controller = "Autmobile", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "OilChange", // Route name
                "OilChange/{action}", // URL with parameters
                new {controller = "OilChange", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "Transit", // Route name
                "Transit/{action}", // URL with parameters
                new {controller = "Transit", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "Repair", // Route name
                "Repair/{action}", // URL with parameters
                new {controller = "Repair", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "FuelConsume", // Route name
                "FuelConsume/{action}", // URL with parameters
                new {controller = "FuelConsume", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "home", // Route name
                "Home/{action}", // URL with parameters
                new {controller = "Home", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "driver", // Route name
                "Driver/{action}", // URL with parameters
                new {controller = "Driver", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "Department", // Route name
                "Department/{action}", // URL with parameters
                new {controller = "Department", action = "Login"} // Parameter defaults
                );

            routes.MapRoute(
                "FuelCard", // Route name
                "FuelCard/{action}", // URL with parameters
                new {controller = "FuelCard", action = "Login"} // Parameter defaults
                );
            routes.MapRoute(
                "reporter", // Route name
                "Reporter/{action}", // URL with parameters
                new {controller = "Reporter", action = "Login"} // Parameter defaults
                );

            routes.MapRoute(
                "TrafficCard", // Route name
                "TrafficCard/{action}", // URL with parameters
                new {controller = "TrafficCard", action = "Login"});

            routes.MapRoute(
                "Incident", // Route name
                "Incident/{action}", // URL with parameters
                new { controller = "Incident", action = "Login" });

            routes.MapRoute(
                     "setting", // Route name
                     "Setting/{action}", // URL with parameters
                     new { controller = "Setting", action = "Login" });
            routes.MapRoute(
         "Color", // Route name
         "Color/{action}", // URL with parameters
         new { controller = "Color", action = "Login" });

            routes.MapRoute(
 "AutomobileClass", // Route name
 "AutomobileClass/{action}", // URL with parameters
 new { controller = "AutomobileClass", action = "Login" });

                        routes.MapRoute(
 "StatisticsAnalysis", // Route name
 "StatisticsAnalysis/{action}", // URL with parameters
 new { controller = "StatisticsAnalysis", action = "Login" });



            
            routes.MapRoute(
                "Localization", // Route name
                "{lang}/{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "login", id = UrlParameter.Optional} // Parameter defaults
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Account", action = "Login", id = UrlParameter.Optional} // Parameter defaults
                );
      
        }
    }
}
