using System;

namespace AG.Loggers
{
    public class AppendedEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly LogLevel LogLevel;
        public readonly int MsgCode;

        public AppendedEventArgs(string message, LogLevel logLevel, int msgCode = 0)
        {
            Message = message;
            LogLevel = logLevel;
            MsgCode = msgCode;
        }
    }
}
