using System;

namespace AG
{
    public static class ExceptionWrapper
    {
        public delegate T Returner<T>();
        public delegate T ExceptionConverter<T>(Exception exception);

        public static Exception TryGetValue<T>(Returner<T> returnerDelegate, out T gottenValue)
        {
            try
            {
                gottenValue = returnerDelegate();
                return null;
            }
            catch (Exception ex)
            {
                gottenValue = default(T);
                return ex;
            }
        }
        public static string TryGetValueOrExceptionString<T>(Returner<T> returnerDelegate, ExceptionConverter<string> exceptionConverter)
        {
            Exception exception;
            T gottenValue;
            if ((exception = TryGetValue<T>(returnerDelegate, out gottenValue)) == null)
            {
                return gottenValue.ToString();
            }
            else
            {
                return exceptionConverter(exception);
            }
        }
    }
}
