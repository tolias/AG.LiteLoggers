using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AG.AssemblyInfo;
using System.Diagnostics;

namespace AG.Loggers
{
    public class VSOutputLogger : LoggerBaseImplementation
    {
        public VSOutputLogger(LogLevel logLevel)
            : base(logLevel)
        {
        }

        protected override void WriteToLog(LogLevel logLevel, string message, int msgCode = 0)
        {
            lock (this)
            {
                Trace.WriteLine(this.FormatMessage(logLevel, message, msgCode));
            }
        }
    }
}
