using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AutomobilMng.Infrastructure
{
    public class Result
    {
        public bool Success { get; set; }
        public string Description { get; set; }
    }
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        #region Logging
        /// <summary>
        /// Access to the log4Net logging object
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string StopwatchKey = "DebugLoggingStopWatch";

        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (log.IsDebugEnabled)
            {
                var loggingWatch = Stopwatch.StartNew();
                filterContext.HttpContext.Items.Add(StopwatchKey, loggingWatch);

                var message = new StringBuilder();
                message.Append(string.Format("Executing controller {0}, action {1}",
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName));

                log.Debug(message);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult)
            {

                //if (filterContext.HttpContext.Response.StatusCode !=(int) HttpStatusCode.OK)
                //    throw new Exception(String.Format(
                //    "Server error (HTTP {0}: {1}).",
                //    response.StatusCode,
                //    response.StatusDescription));
                //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                //object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                //Response jsonResponse
                //= objResponse as Response;
                //return jsonResponse;

                JsonResult jsonResult = (JsonResult)filterContext.Result;
               // var Data = JsonConvert.DeserializeObject(jsonResult.Data);
              
                try
                {
                    dynamic g = jsonResult.Data;
                    var f = g.success;
                   // var n = jsonResult.Data.success;
                    Result result = new JavaScriptSerializer().Deserialize<Result>(jsonResult.Data.ToString());
                }
                catch { }
            }
            if (log.IsDebugEnabled)
            {
                if (filterContext.HttpContext.Items[StopwatchKey] != null)
                {
                    var loggingWatch = (Stopwatch)filterContext.HttpContext.Items[StopwatchKey];
                    loggingWatch.Stop();

                    long timeSpent = loggingWatch.ElapsedMilliseconds;

                    var message = new StringBuilder();
                    message.Append(string.Format("Finished executing controller {0}, action {1} - time spent {2}",
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                        filterContext.ActionDescriptor.ActionName,
                        timeSpent));

                    log.Debug(message);
                    filterContext.HttpContext.Items.Remove(StopwatchKey);
                }
            }
        }
    }
}