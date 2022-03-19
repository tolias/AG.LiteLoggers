using System;

namespace AG.Loggers.Helpers
{
    public static class ConditionalLogging
    {
        public static bool ExecuteIfParamIsNotNull<TParam>(TParam param, Action action)
            where TParam : class
        {
            if (param != null)
            {
                action();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void LogOrThrowException(LoggerBase logger, LogLevel logLevel, string errorMessage)
        {
            if (!ExecuteIfParamIsNotNull(logger, () => logger.Log(logLevel, errorMessage)))
            {
                throw new ApplicationException(errorMessage);
            }
        }

        public static void LogExceptionOrThrowIt(LoggerBase logger, LogLevel logLevel, Exception exception, string errorMessage)
        {
            if (!ExecuteIfParamIsNotNull(logger, () => logger.Log(logLevel, exception, errorMessage)))
            {
                throw new ApplicationException(errorMessage, exception);
            }
        }
    }
}
