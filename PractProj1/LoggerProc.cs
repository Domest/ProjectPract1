using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PractProj1
{
    public class LoggerProc
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static void createLoggerConfig()
        {
            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget();
            config.AddTarget("logfile", fileTarget);

            fileTarget.FileName = "../../logs/${longdate:cached=true}.log";
            string NCSPLayout = @"${date:format=dd.MM.yyyy | HH\:mm\:ss.fffffff} — ${message}";
            fileTarget.Layout = NCSPLayout;
            fileTarget.ArchiveFileName = System.Configuration.ConfigurationManager.AppSettings["LogArchiveDirectory"];
            fileTarget.ArchiveAboveSize = 10485760;
            fileTarget.ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Rolling;
            fileTarget.MaxArchiveFiles = 10;

            LoggingRule rule = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }
        //public void CheckLogs()
        //{
        //    //string GetFilesDir = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\logs");
        //    string[] PathLogFiles = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\logs"));
        //    foreach (string a in PathLogFiles)
        //    {
        //        string[] Splitter = a.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries); 
        //    }
        //        //GetFilesDir.Split(new char[] { '-', '.' }, StringSplitOptions.RemoveEmptyEntries);
        //}
    }
}
