using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AutomobilMng.Log
{
    public class LogData
    {
        public LogData()
        {
            Data = new Dictionary<string, string>();
        }
        public Dictionary<string, string> Data { get; set; }

        public int? ObjectID { get; set; }

        public string Message { get; set; }

        public void GetClassData(Type classType, object o)
        {

            if (Attribute.IsDefined(classType, typeof(LogAttribute), true))
                foreach (var attr in classType.GetCustomAttributes(typeof(LogAttribute), true).OfType<LogAttribute>())
                {
                    int oid;
                    if (int.TryParse(attr.ObjectID, out oid) && !ObjectID.HasValue)
                        ObjectID = oid;

                    AddToDic("Owner", attr.Owner);
                    AddToDic("Tag", attr.Tag);
                }
            if (o != null)
                foreach (var item in classType.GetProperties())
                {
                    if (Attribute.IsDefined(item, typeof(LogAttribute), true))
                    {
                        var logAttr = item.GetCustomAttributes(typeof(LogAttribute), true).OfType<LogAttribute>().First();
                        AddToDic(logAttr.Name, item.GetValue(o, new object[] { }));
                    }
                    if (item.PropertyType.IsClass && !(item.GetValue(o, new object[] { }) is IEnumerable))
                        GetClassData(item.PropertyType, item.GetValue(o, new object[] { }));

                }
        }

        public void GetMethodData(MethodInfo method)
        {
            if (Attribute.IsDefined(method, typeof(LogAttribute), true))
                foreach (var attr in method.GetCustomAttributes(typeof(LogAttribute), true).OfType<LogAttribute>())
                {
                    int oid;
                    if (int.TryParse(attr.ObjectID, out oid) && !ObjectID.HasValue)
                        ObjectID = oid;
                    if (Message == null)
                        if (attr.Message != null)
                            Message = attr.Message;

                    AddToDic("Owner", attr.Owner);
                    AddToDic("Tag", attr.Tag);
                }
        }

        public void AddToDic(string key, object data)
        {
            if (key.ToLower() == "tag" && data != null)
                data = data.ToString().ToLower();
            if (key.ToLower() == "tag" && Data.ContainsKey("tag") && data != null)
                Data[key.ToLower()] += ", " + data.ToString();
            if (key != null && !Data.ContainsKey(key.ToLower()) && data != null)
                Data.Add(key.ToLower(), data.ToString());

        }
    }
}