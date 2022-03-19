using System;

namespace AG.Loggers
{
    public abstract class LoggerBase
    {
        public delegate void AppendedHandler(LoggerBase logger, AppendedEventArgs e);

        public bool DefaultIncludeStackTraceForException = true;
        public event AppendedHandler Appended;
        public string FilterForEvents;

        public abstract LogLevel LogLevel { get; set; }

        public delegate string StringReturner();

        public abstract void Log(LogLevel logLevel, StringReturner stringReturner, int msgCode = 0);

        public abstract void Log(LogLevel logLevel, string message, int msgCode = 0);

        public virtual void Log(LogLevel logLevel, string format, params object[] args)
        {
            Log(logLevel, () => string.Format(format, args));
        }

        public virtual void Log(LogLevel logLevel, Exception e)
        {
            Log(logLevel, e, DefaultIncludeStackTraceForException);
        }

        public virtual void Log(LogLevel logLevel, Exception e, bool includeStackTrace)
        {
            Log(logLevel, () => "Exception: " + ExceptionInfoProvider.GetExceptionInfo(e, includeStackTrace) + "\r\n\r\n");
        }

        public virtual void Log(LogLevel logLevel, Exception e, string altMessage, int msgCode = 0)
        {
            Log(logLevel, e, altMessage, DefaultIncludeStackTraceForException, msgCode);
        }

        public virtual void Log(LogLevel logLevel, Exception e, string altMessage, bool includeStackTrace, int msgCode = 0)
        {
            Log(logLevel, () => altMessage + ". Exception: " + ExceptionInfoProvider.GetExceptionInfo(e, includeStackTrace) + "\r\n\r\n", msgCode);
        }

        public virtual void Log(LogLevel logLevel, Exception e, string format, params object[] args)
        {
            Log(logLevel, e, DefaultIncludeStackTraceForException, format, args);
        }

        public virtual void Log(LogLevel logLevel, Exception e, bool includeStackTrace, string format, params object[] args)
        {
            Log(logLevel, () => string.Format(format, args) + ". Exception: " + ExceptionInfoProvider.GetExceptionInfo(e, includeStackTrace) + "\r\n\r\n");
        }

        protected void OnAppended(LogLevel logLevel, string message, int msgCode = 0)
        {
            if (this.Appended != null)
            {
                if (!string.IsNullOrEmpty(this.FilterForEvents) && message.Contains(FilterForEvents))
                {
                    return;
                }
                this.Appended(this, new AppendedEventArgs(message, logLevel, msgCode));
            }
        }
    }
}
