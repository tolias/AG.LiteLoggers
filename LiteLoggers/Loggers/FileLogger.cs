using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AG.AssemblyInfo;
using System.Threading;
using System.Security;
using System.Text.RegularExpressions;
using AG.PathStringOperations;

namespace AG.Loggers
{
    public class FileLogger : LoggerBaseImplementation, IDisposable
    {
        private StreamWriter _logFileStream;
        private string _logFileName;

        protected Encoding utf8Encoding = new UTF8Encoding(true, false);

        private WriteLogOptions _writeLogMode = WriteLogOptions.DontLockFile;

        public delegate void StringBuilderProcessor(StringBuilder stringBuilder);
        public event StringBuilderProcessor HeaderLinesAppending;

        private object _fileSynchronizationContext = new object();

        public FileLogger(string logFileName, LogLevel logLevel, WriteLogOptions writeLogMode = WriteLogOptions.DontLockFile, bool logProgramInfo = true)
        {
            this._logFileName = logFileName;
            this.LogLevel = logLevel;
            if (logProgramInfo)
            {
                LogProgramInfo();
            }
        }

        public object FileSynchronizationContext { get { return _fileSynchronizationContext; } }

        public WriteLogOptions WriteLogMode
        {
            get { return _writeLogMode; }
            set { _writeLogMode = value; }
        }

        public string LogFileName
        {
            get { return _logFileName; }
            set
            {
                if (value != _logFileName)
                {
                    lock (_fileSynchronizationContext)
                    {
                        _logFileName = value;
                        if (_logFileStream != null)
                        {
                            _logFileStream.Close();
                            OpenFile();
                        }
                    }
                }
            }
        }

        protected override void WriteToLog(LogLevel logLevel, string message, int msgCode = 0)
        {
            lock (_fileSynchronizationContext)
            {
                if (_logFileStream == null)
                    OpenFile();

                try
                {
                    _logFileStream.WriteLine(this.FormatMessage(logLevel, message, msgCode));

                    switch (this.WriteLogMode)
                    {
                        case WriteLogOptions.DontLockFile:
                            _logFileStream.Close();
                            _logFileStream = null;
                            break;
                        case WriteLogOptions.LockAndFlushAfterEveryWrite:
                            //a bad solution... but _logFileStream.Flush() doesn't work. It doesn't want to flush writed data to a file immediatelly
                            _logFileStream.Close();
                            _logFileStream = new StreamWriter(_logFileName, true, utf8Encoding);
                            //_logFileStream.Flush();
                            break;
                    }
                }
                catch
                {
                    // ignored, because we can't throw exception in logger. It can crash program work
                }
            }
        }

        public void LogProgramInfo()
        {
            try
            {
                StringBuilder fileHeaderLines = new StringBuilder();
                fileHeaderLines.Append("Program: ");
                try
                {
                    fileHeaderLines.Append(ProgramInfo.FullProgramName);
                }
                catch (InvalidOperationException ex)
                {
                    fileHeaderLines.Append(ex.Message);
                }
                fileHeaderLines.Append(" | OS: ");
                fileHeaderLines.Append(Environment.OSVersion.Platform);
                fileHeaderLines.Append(' ');
                fileHeaderLines.Append(Environment.OSVersion.Version);
                fileHeaderLines.Append(' ');
                fileHeaderLines.Append(Environment.OSVersion.ServicePack);
                if (HeaderLinesAppending != null)
                {
                    HeaderLinesAppending(fileHeaderLines);
                }
                fileHeaderLines.Append(" | LogLevel: ");
                fileHeaderLines.Append(this.LogLevel);
                Log(LogLevel.Debug, fileHeaderLines.ToString());
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, ex, "Couldn't log program info");
            }
        }

        protected void OpenFile()
        {
            //if (!File.Exists(_logFileName))
            //{
            //}
            int openingNewFileAttempts = 0;
        tryToOpenFileAgain:
            try
            {
                _logFileStream = new StreamWriter(_logFileName, true, utf8Encoding);
                if (_writeLogMode == WriteLogOptions.LockAndFlushAfterEveryWrite)
                {
                    _logFileStream.AutoFlush = true;
                }
            }
            catch (DirectoryNotFoundException)
            {
                try
                {
                    string logDir = Path.GetDirectoryName(_logFileName);
                    Directory.CreateDirectory(logDir);
                    goto tryToOpenFileAgain;
                }
                catch { }
            }
            catch (SystemException systemException)
            {
                if(systemException is IOException
                    || systemException is UnauthorizedAccessException
                    || systemException is SecurityException)
                {
                    if (openingNewFileAttempts > 0)
                    {
                        return;
                    }

                    //string fileNameWithoutExt = Path.GetFileNameWithoutExtension(_logFileName);
                    //string fileExt = Path.GetExtension(_logFileName);
                    //int fileNameNumber;
                    //string fileNameExceptNumber;
                    //GetFileNameNumber(fileNameWithoutExt, out fileNameNumber, out fileNameExceptNumber);
                    //fileNameNumber++;
                    //_logFileName = string.Format("{0}({1}){2}", fileNameExceptNumber, fileNameNumber, fileExt);

                    _logFileName = ExtendedPath.GetIncrementedPath(_logFileName, "({0})");

                    if (!File.Exists(_logFileName))
                    {
                        openingNewFileAttempts++;
                    }
                    goto tryToOpenFileAgain;
                }
            }
        }

        private bool GetFileNameNumber(string fileName, out int number, out string fileNameExceptNumber)
        {
            number = 1;
            fileNameExceptNumber = fileName;
            if (!fileName.EndsWith(")"))
                return false;

            int firstParentesisIndex = fileName.LastIndexOf('(');
            if (firstParentesisIndex == -1)
                return false;

            string sNumber = fileName.Substring(firstParentesisIndex, fileName.Length - firstParentesisIndex - 1);

            if (!int.TryParse(sNumber, out number))
            {
                number = 1;
                return false;
            }
            fileNameExceptNumber = fileName.Substring(0, firstParentesisIndex);
            return true;
        }

        public override string ToString()
        {
            return base.ToString() + " | " + this._logFileName;
        }

        public enum WriteLogOptions
        {
            DontLockFile,
            LockAndFlushAfterEveryWrite,
            Lock
        }

        public void Close()
        {
            lock (_fileSynchronizationContext)
            {
                try
                {
                    if(_logFileStream!= null)
                        _logFileStream.Close();
                }
                catch { }
                _logFileStream = null;
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
