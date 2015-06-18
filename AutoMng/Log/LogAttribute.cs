using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AutomobilMng.Log
{
    public enum Subject
    {
        UserLogin = 101,
        UserLogout = 102,
        UserMenu = 103,
        UserRegister = 104,
        UserShow = 105,
        UserEdit = 106,
        UserDelete = 107,
        UserRoles = 108,

        AutomobileMenu = 201,
        AutomobileShow = 202,
        AutomobileNew = 203,
        AutomobileEdit = 204,
        AutomobileDelete = 205,
        AutomobileReport = 206,
        AutomobileUnDeliveryShow = 207,
        AutomobileStatisticsAnalysis = 208,
        AutomobileChangeStatus = 209,

        ColorMenu = 301,
        ColorShow = 302,
        ColorNew = 303,
        ColorEdit = 304,
        ColorDelete = 305,

        AutomobileClassMenu = 401,
        AutomobileClassShow = 402,
        AutomobileClassNew = 403,
        AutomobileClassEdit = 404,
        AutomobileClassDelete = 405,

        OilChangeMenu = 501,
        OilChangeShow = 502,
        OilChangeNew = 503,
        OilChangeEdit = 504,
        OilChangeDelete = 505,
        OilChangeReport = 506,



        DepartmentMenu = 606,
        DepartmentShow = 606,
        DepartmentNew = 606,
        DepartmentEdit = 606,
        DepartmentDelete = 606,

        DriverMenu = 701,
        DriverShow = 702,
        DriverNew = 703,
        DriverEdit = 704,
        DriverDelete = 705,


        FuelMenu = 800,
        FuelShow = 801,
        FuelNew = 802,
        FuelEdit = 803,
        FuelDelete = 804,
        FuelReport = 805,
        FuelReportCompareFuel = 806,

        IncidentMenu = 900,
        IncidentShow = 901,
        IncidentNew = 902,
        IncidentEdit = 903,
        IncidentDelete = 904,
        IncidentReport = 906,

        RepairMenu = 150,
        RepairShow = 151,
        RepairNew = 152,
        RepairEdit = 153,
        RepairDelete = 154,
        RepairChartAutomobile = 155,
        RepairChartWorkshopReferral = 156,

        SaveSetting = 250,
        ShowSetting = 251,

        TrafficCardMenu = 350,
        TrafficCardShow = 351,
        TrafficCardNew = 352,
        TrafficCardEdit = 353,
        TrafficCardDelete = 354,
    }

    public class LogAttribute : Attribute
    {
        public LogAttribute()
        {

        }
        public string Owner { get; set; }

        public string Name { get; set; }

        public string ObjectID { get; set; }

        public static int ApplicationID { get; set; }

        public static string ApplicationName { get; set; }

        public string Tag { get; set; }

        public string Message { get; set; }

        public static List<KeyValuePair<string, string>> GetProperties<T>(T obj, string objectid, string result)
        {
            List<KeyValuePair<string, string>> dic = new List<KeyValuePair<string, string>>();
            dic.Add(new KeyValuePair<string, string>("objectid", ((int)Subject.OilChangeNew).ToString()));
            dic.Add(new KeyValuePair<string, string>("result", "success"));

           // T myClass = default(T); ;
            var type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            if (obj!=null)
                foreach (PropertyInfo property in properties)
                {
                    dic.Add(new KeyValuePair<string, string>(property.Name, property.GetValue(obj, null) == null ? "" : property.GetValue(obj, null).ToString()));

                    // dic.Add(property.Name, property.GetValue(myClass, null) == null ? "" : property.GetValue(myClass, null).ToString());
                    // Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(myClass, null));
                }
            return dic;
        }
    
    }
}