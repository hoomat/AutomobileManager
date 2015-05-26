using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AutomobilMng.Log
{
    public class Logger
    {
        public enum CriticalityLevel { Debug, Info, Warn, Error, Fatal, Trace }

        static Uri configAddress;

        public static void ReInitialize(Uri configAddress, int updateInterval = 5000)
        {
            XmlConfigurator.Configure(configAddress);
            Logger.configAddress = configAddress;
        }

        public static void Initialize(Uri configAddress, int updateInterval = 5000)
        {
            //if (updateTimer == null)
            {
                ReInitialize(configAddress, updateInterval);
            }
        }

  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="level"></param>
        /// <param name="applicationID"></param>
        /// <param name="objectID"></param>
        /// <param name="message"></param>
        /// <param name="exception">Set null if no exception</param>
        /// <param name="extendedData"></param>
        public static void Send(Type sender, CriticalityLevel level, string username, string message, Exception exception, params KeyValuePair<string, string>[] extendedData)
        {
            log4net.ThreadContext.Properties.Clear();
            log4net.ThreadContext.Properties["user"] = username;
            if (extendedData != null)
                foreach (var item in extendedData)
                    log4net.ThreadContext.Properties[item.Key.ToLower()] = item.Value;
            log4net.ILog log = log4net.LogManager.GetLogger(sender);
            switch (level)
            {
                case CriticalityLevel.Debug:
                    if (exception != null)
                        log.Debug(message, exception);
                    else
                        log.Debug(message);
                    break;
                case CriticalityLevel.Info:
                    if (exception != null)
                        log.Info(message, exception);
                    else
                        log.Info(message);
                    break;
                case CriticalityLevel.Warn:
                    if (exception != null)
                        log.Warn(message, exception);
                    else
                        log.Warn(message);
                    break;
                case CriticalityLevel.Error:
                    if (exception != null)
                        log.Error(message, exception);
                    else
                        log.Error(message);
                    break;
                case CriticalityLevel.Fatal:
                    if (exception != null)
                        log.Fatal(message, exception);
                    else
                        log.Fatal(message);
                    break;

            }
        }

        public static void Send(Type sender, CriticalityLevel level, int applicationID, string applicationName, int eventID, string message, Exception exception, params KeyValuePair<string, string>[] extendedData)
        {
            //if (updateTimer == null)
            //    return;
            //throw new InvalidOperationException("Logger has not beed initialized");
            log4net.ThreadContext.Properties.Clear();
            log4net.ThreadContext.Properties["applicationid"] = applicationID;
            log4net.ThreadContext.Properties["applicationname"] = applicationName;
            log4net.ThreadContext.Properties["eventid"] = eventID;
            if (extendedData != null)
                foreach (var item in extendedData)
                {
                    log4net.ThreadContext.Properties[item.Key.ToLower()] = item.Value;
                }
            log4net.ILog log = log4net.LogManager.GetLogger(sender);
            switch (level)
            {
                case CriticalityLevel.Debug:
                    if (exception != null)
                        log.Debug(message, exception);
                    else
                        log.Debug(message);
                    break;
                case CriticalityLevel.Info:
                    if (exception != null)
                        log.Info(message, exception);
                    else
                        log.Info(message);
                    break;
                case CriticalityLevel.Warn:
                    if (exception != null)
                        log.Warn(message, exception);
                    else
                        log.Warn(message);
                    break;
                case CriticalityLevel.Error:
                    if (exception != null)
                        log.Error(message, exception);
                    else
                        log.Error(message);
                    break;
                case CriticalityLevel.Fatal:
                    if (exception != null)
                        log.Fatal(message, exception);
                    else
                        log.Fatal(message);
                    break;

            }
        }

        public static void Send(Type sender, CriticalityLevel level,string username, List<object> logObjects, Exception exception, MethodInfo method, params KeyValuePair<string, string>[] extraData)
        {
            //if (logObjects == null)
            //    return;
            LogData ld = new LogData();
            foreach (var item in logObjects)
            {
                if (item != null)
                    ld.GetClassData(item.GetType(), item);
            }

            if (method != null)
                ld.GetMethodData(method);
            if (extraData != null)
                foreach (var item in extraData)
                {

                    ld.AddToDic(item.Key.ToLower(), item.Value);
                }
            Send(sender, level,username, ld.Message, exception, ld.Data.ToArray());
        }

        public static bool IsDebugEnabled(Type sender)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(sender);
            return log.IsDebugEnabled;
        }
    }
}