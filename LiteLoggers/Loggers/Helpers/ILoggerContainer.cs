using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Loggers.Helpers
{
    public interface ILoggerContainer
    {
        LoggerBase Logger { set; }
    }
}
