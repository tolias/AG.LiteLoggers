using System;

namespace AG.Loggers
{
    public class ConsoleLogger : LoggerBaseImplementation
    {
        public LevelConsoleColors _consoleLevelColors;
        public LevelConsoleColors ConsoleLevelColors
        {
            get { return _consoleLevelColors; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("ConsoleLevelColors", "ConsoleLevelColors can't be null");
                }
                _consoleLevelColors = value;
            }
        }
        public ConsoleLogger(LogLevel logLevel)
            : this(logLevel, LevelConsoleColors.Default)
        {
        }
        public ConsoleLogger(LogLevel logLevel, LevelConsoleColors consoleLevelColors)
        {
            this.LogLevel = logLevel;
            ConsoleLevelColors = consoleLevelColors;
        }

        protected override void WriteToLog(LogLevel logLevel, string message, int msgCode = 0)
        {
            string formattedMessage = this.FormatMessage(logLevel, message, msgCode);
            var consoleLogColor = _consoleLevelColors.GetConsoleColor(logLevel);
            lock (this)
            {
                var previousConsoleColor = BgAndForeColor.Current;
                BgAndForeColor.Current = consoleLogColor;
                Console.WriteLine(formattedMessage);
                BgAndForeColor.Current = previousConsoleColor;
            }
        }

        public class LevelConsoleColors
        {
            public BgAndForeColor Error;
            public BgAndForeColor Warning;
            public BgAndForeColor Info;
            public BgAndForeColor Debug;
            public BgAndForeColor None;

            public BgAndForeColor GetConsoleColor(LogLevel logLevel)
            {
                switch (logLevel)
                {
                    case LogLevel.Error:
                        return Error;
                    case LogLevel.Warning:
                        return Warning;
                    case LogLevel.Info:
                        return Info;
                    case LogLevel.Debug:
                        return Debug;
                    case LogLevel.None:
                    default:
                        return None;
                }
            }

            public static readonly LevelConsoleColors Default;

            static LevelConsoleColors()
            {
                Default = new LevelConsoleColors
                {
                    Error = new BgAndForeColor(ConsoleColor.Black, ConsoleColor.Red),
                    Warning = new BgAndForeColor(ConsoleColor.Black, ConsoleColor.Yellow),
                    Info = new BgAndForeColor(ConsoleColor.Black, ConsoleColor.Cyan),
                    Debug = new BgAndForeColor(ConsoleColor.Black, ConsoleColor.Gray),
                    None = new BgAndForeColor(ConsoleColor.Black, ConsoleColor.Magenta)
                };
            }
        }

        public class BgAndForeColor
        {
            public ConsoleColor BgColor;
            public ConsoleColor ForeColor;

            public BgAndForeColor(ConsoleColor bgColor, ConsoleColor foreColor)
            {
                BgColor = bgColor;
                ForeColor = foreColor;
            }

            public static BgAndForeColor Current
            {
                get
                {
                    return new BgAndForeColor(Console.BackgroundColor, Console.ForegroundColor);
                }
                set
                {
                    Console.BackgroundColor = value.BgColor;
                    Console.ForegroundColor = value.ForeColor;
                }
            }

            public override string ToString()
            {
                return string.Format("BgColor={0}, ForeColor={1}", BgColor, ForeColor);
            }
        }
    }
}
