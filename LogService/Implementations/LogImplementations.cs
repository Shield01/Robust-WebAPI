using LogService.Abstractions;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogService.Implementations
{
    public class LogImplementations : ILogImplementations
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LogImplementations()
        {

        }

        public void DebugMessage(string message)
        {
            logger.Debug(message);
        }

        public void ErrorMessage(string message)
        {
            logger.Error(message);
        }

        public void InfoMessage(string message)
        {
            logger.Info(message);
        }

        public void WarningMessage(string message)
        {
            logger.Warn(message);
        }
    }
}
