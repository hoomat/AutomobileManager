using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Log
{
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

    
    }
}