using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Loggers
{
    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4,
        Errors = Error,
        Warnings = Warning
    }
}
