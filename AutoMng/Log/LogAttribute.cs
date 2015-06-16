using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AutomobilMng.Log
{
    public enum Subject
    {
        OilChangeMenu = 5001,
        OilChangeShow = 5002,
        OilChangeNew = 5003,
        OilChangeEdit = 5004,
        OilChangeDelete = 5005,
        OilChangeReport = 5006,
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