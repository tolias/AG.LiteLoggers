using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Loggers
{
    public class Loggers : LoggerBase, IDisposable
    {
        public List<LoggerBaseImplementation> InnerLoggers;

        public Loggers(params LoggerBaseImplementation[] loggers)
        {
            //try
            //{
            //    this.LogLevel = loggers[0].LogLevel;
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    throw new InvalidOperationException("You should pass at least one logger in the constructor");
            //}
            InnerLoggers = new List<LoggerBaseImplementation>(loggers);
        }

        public override LogLevel LogLevel
        {
            get
            {
                return InnerLoggers[0].LogLevel;
            }
            set
            {
                foreach (var logger in InnerLoggers)
                {
                    logger.LogLevel = value;
                }
            }
        }

        public void AddLogger(LoggerBaseImplementation logger)
        {
            InnerLoggers.Add(logger);
        }

        public override void Log(LogLevel logLevel, LoggerBase.StringReturner stringReturner, int msgCode = 0)
        {
            foreach (LoggerBaseImplementation logger in InnerLoggers)
            {
                logger.Log(logLevel, stringReturner, msgCode);
            }
        }

        public override void Log(LogLevel logLevel, string message, int msgCode = 0)
        {
            foreach (LoggerBaseImplementation logger in InnerLoggers)
            {
                logger.Log(logLevel, message);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (LoggerBaseImplementation logger in InnerLoggers)
            {
                sb.AppendLine(logger.ToString());
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            foreach (var logger in InnerLoggers)
            {
                IDisposable disposableLogger = logger as IDisposable;
                if (disposableLogger != null)
                {
                    disposableLogger.Dispose();
                }
            }
        }
    }
}
