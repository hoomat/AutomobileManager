using DAL;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AutomobilMng.Log
{
    class DatabaseAppender : AppenderSkeleton
    {


        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            StringWriter sw = new StringWriter();
            RenderLoggingEvent(sw, loggingEvent);
            XDocument doc = XDocument.Parse(sw.ToString());
            Trap t = new Trap();
            XElement ev = doc.Element("event");
            t.TimeSpan = DateTime.Parse(ev.Attribute("timestamp").Value);
            t.Logger = ev.Attribute("logger").Value;
            t.Thread = int.Parse(ev.Attribute("thread").Value);
            t.Level = ev.Attribute("level").Value;
           // t.Username = ev.Attribute("username").Value;
           // t.Username = t.Properties.FirstOrDefault(p => p.Key.ToLower() == "user" && p.Key != "objectid")
            t.Domain = ev.Attribute("domain").Value;
            t.Message = ev.Element("message").Value;
           
            foreach (var item in ev.Element("properties").Elements("data"))
            {
                t.Properties.Add(new TrapDetail { Key = item.Attribute("name").Value, Value = item.Attribute("value").Value });
            }
            var user = t.Properties.FirstOrDefault(p => p.Key.ToLower() == "user");
            if (user != null)
                t.Username = user.Value;
            XElement exception = ev.Element("exception");
            if (exception != null)
                t.Properties.Add(new TrapDetail { Key = "exception", Value = exception.Value });

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
             
                DAL.Log log = new DAL.Log
                  {
                      DateTime = t.TimeSpan,
                      Level = t.Level,
                      Message = t.Message,
                      Username = t.Username,
                      LogDetails=new List<LogDetail>()
                  };
                foreach (var item in t.Properties.Where(p => p.Key.ToLower() != "log4net:HostName" && p.Key != "user"))
                {
                    LogDetail ld = new LogDetail {  Name = item.Key, Value = item.Value };
                    log.LogDetails.Add(ld);
                }
                db.Logs.Add(log);
                db.SaveChanges();
            }
        }
    }
}