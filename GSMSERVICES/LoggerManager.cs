using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES
{
    public class LoggerManager
    {

        private static Logger logger;
        public static void InitializeLogger()
        {

            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                Name = "fileTarget",
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${longdate} ${uppercase:${level}} ${message}"
            };
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);
            LogManager.Configuration = config;
            logger = LogManager.GetLogger("MainLog");
        }
        public static void LogInfo(string message)
        {
            logger.Info(message);
        }
        public static void LogError(string message)
        {
            logger.Error(message);
        }
        public static void LogTrace(string message)
        {
            logger.Trace(message);
        }
    }
}
