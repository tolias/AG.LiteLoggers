using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AG.Loggers
{
    public class TextBoxLogger : LoggerBaseImplementation
    {
        private TextBox _loggerTextBox;
        private delegate void AppendTextDel(string text);
        private AppendTextDel appendText;

        public TextBoxLogger(LogLevel logLevel)
        {
            this.LogLevel = logLevel;
        }

        public TextBoxLogger(TextBox loggerTextBox, LogLevel logLevel)
        {
            this.LoggerTextBox = loggerTextBox;
            this.LogLevel = logLevel;
        }

        public TextBox LoggerTextBox
        {
            get { return _loggerTextBox; }
            set
            {
                this._loggerTextBox = value;
                this.appendText = value.AppendText;
            }
        }

        protected override void WriteToLog(LogLevel logLevel, string message, int msgCode = 0)
        {
            try
            {
                string msg = this.FormatMessage(logLevel, message, msgCode) + "\r\n";
                if (this._loggerTextBox.InvokeRequired)
                {
                    this._loggerTextBox.Invoke(this.appendText, msg);
                }
                else
                {
                    this._loggerTextBox.AppendText(msg);
                }
            }
            catch { }
        }
    }
}
