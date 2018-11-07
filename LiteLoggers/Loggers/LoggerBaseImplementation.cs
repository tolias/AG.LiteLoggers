using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AG.AssemblyInfo;

namespace AG.Loggers
{
    public abstract class LoggerBaseImplementation : LoggerBase
    {
        public override LogLevel LogLevel { get; set; }
        public delegate string FormatMessageDelegate(LogLevel logLevel, string message, int msgCode = 0);
        public FormatMessageDelegate FormatMessage;
        public DateTimeKind DateTimeKind;
        //public delegate string GetLogLevelNameDelegate(LogLevel logLevel);
        //public GetLogLevelNameDelegate GetLogLevelName = DefaultGetLogLevelName;
        //public LogTypeWord TypeWord = new LogTypeWord("INFO", "WARNING", "ERROR", "DEBUG");

        protected LoggerBaseImplementation()
        {
            FormatMessage = DefaultFormatMessage;
        }

        protected LoggerBaseImplementation(LogLevel logLevel)
            : this()
        {
            this.LogLevel = logLevel;
        }

        private string DefaultFormatMessage(LogLevel logLevel, string message, int msgCode = 0)
        {
            System.Threading.Thread currentThread = System.Threading.Thread.CurrentThread;
            var now = DateTimeKind == System.DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
            //return string.Format("{2} [{3}_{4}] {0}: {1}", OnGetLogLevelName(logLevel), message, now.ToString(@"yy\.MM\.dd HH\:mm\:ss\,fff"), currentThread.ManagedThreadId, currentThread.Name);
            var str = $"{now.ToString(@"yy\.MM\.dd HH\:mm\:ss\,fff")} [{currentThread.ManagedThreadId}_{currentThread.Name}] {OnGetLogLevelName(logLevel)}: {message}";
            if (msgCode > 0)
            {
                str += $" ({msgCode})";
            }
            return str;
        }

        public virtual string OnGetLogLevelName(LogLevel logLevel)
        {
            return logLevel.ToString().ToUpper();
        }

        protected abstract void WriteToLog(LogLevel logLevel, string message, int msgCode = 0);

        private void OnWriteToLog(LogLevel logLevel, string message, int msgCode = 0)
        {
            WriteToLog(logLevel, message, msgCode);
            OnAppended(logLevel, message, msgCode);
        }

        public override void Log(LogLevel logLevel, LoggerBase.StringReturner stringReturner, int msgCode = 0)
        {
            if (LogLevel >= logLevel)
            {
                this.OnWriteToLog(logLevel, stringReturner(), msgCode);
            }
        }

        public override void Log(LogLevel logLevel, string message, int msgCode = 0)
        {
            if (LogLevel >= logLevel)
            {
                this.OnWriteToLog(logLevel, message);
            }
        }

        public override string ToString()
        {
            return this.LogLevel.ToString();
        }

        //public class LogTypeWord
        //{
        //    public string Info;
        //    public string Warning;
        //    public string Error;
        //    public string Debug;

        //    public LogTypeWord(string infoWord, string warningWord, string errorWord, string debugWord)
        //    {
        //        this.Info = infoWord;
        //        this.Warning = warningWord;
        //        this.Error = errorWord;
        //        this.Debug = debugWord;
        //    }
        //}
    }
}
