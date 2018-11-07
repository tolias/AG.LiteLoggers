using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Loggers
{
    public class Logger : LoggerBaseImplementation
    {
        public static LoggerBase Current;

        IStringAppender Appender;

        public Logger(IStringAppender appender)
        {
            Appender = appender;
        }

        protected override void WriteToLog(LogLevel logLevel, string message, int msgCode = 0)
        {
            Appender.Append(FormatMessage(logLevel, message, msgCode));
        }
    }
}
