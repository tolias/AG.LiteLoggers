using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AG.Loggers;
using AG.Loggers.Helpers;

namespace AG.PathStringOperations
{
    public static class FileDirectoryManager
    {
        //private const string CLASS_NAME = "FilesArchiver.";

        /// </summary>
        /// 
        /// <param name="action"></param>
        /// <returns>True if directory was created</returns>
        public static bool CreateDirectoryForFile(string fileName, FileAttributes dirAttributes = FileAttributes.Normal, LoggerBase logger = null, bool checkExistense = true)
        {
            string dir = null;
            try
            {
                dir = Path.GetDirectoryName(fileName);
                if (!checkExistense || !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (dirAttributes != FileAttributes.Normal)
                {
                    SetDirectoryAttributes(dir, dirAttributes);
                }
                return true;
            }
            catch (Exception e)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, LogLevel.Error, e, string.Format(Resources.CantCreateDirectory0ForFile1, dir, fileName));
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True if directory existed. If it was created - returns false</returns>
        public static bool CreateDirectoryIfItDoesntExistForFile(string fileName, Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                if (CreateDirectoryForFile(fileName, checkExistense: false))
                {
                    action();
                }
            }
            catch(Exception ex)
            {
                Exception innerException = ex.InnerException;
                while(innerException != null)
                {
                    DirectoryNotFoundException directoryNotFoundException = innerException as DirectoryNotFoundException;
                    if(directoryNotFoundException != null)
                    {
                        if (CreateDirectoryForFile(fileName))
                        {
                            action();
                        }
                        return false;
                    }
                    innerException = innerException.InnerException;
                }
                throw ex;
            }
            return false;
        }

        public static IOException DoActionWithFileAndDirecotryNotFoundExceptionHandling(Action action)
        {
            try
            {
                action();
                return null;
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                return fileNotFoundException;
            }
            catch(DirectoryNotFoundException directoryNotFoundException)
            {
                return directoryNotFoundException;
            }
        }

        public static bool SetDirectoryAttributes(string dirName, FileAttributes attributes, LoggerBase logger = null)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(dirName);
                di.Attributes = attributes;
                return true;
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, LogLevel.Debug, fileNotFoundEx, string.Format("Can't change dirAttributes for folder \"{0}\" to \"{1}\"", dirName, attributes));
            }
            catch (Exception attrException)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, LogLevel.Error, attrException, string.Format("Can't change dirAttributes for folder \"{0}\" to \"{1}\"", dirName, attributes));
            }
            return false;
        }

        public static bool SetFileAttributes(string fileName, FileAttributes attributes, LoggerBase logger = null)
        {
            FileInfo fileInfo;
            return SetFileAttributes(fileName, attributes, out fileInfo, logger);
        }

        public static bool SetFileAttributes(string fileName, FileAttributes attributes, out FileInfo fileInfo, LoggerBase logger = null, LogLevel errorLogLevel = LogLevel.Error)
        {
            try
            {
                fileInfo = new FileInfo(fileName);
                fileInfo.Attributes = attributes;
                return true;
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, errorLogLevel, fileNotFoundEx, string.Format("Can't change dirAttributes for file \"{0}\" to \"{1}\"", fileName, attributes));
            }
            catch (Exception attrException)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, errorLogLevel, attrException, string.Format("Can't change dirAttributes for file \"{0}\" to \"{1}\"", fileName, attributes));
            }
            fileInfo = null;
            return false;
        }

        public static bool SetFileAttributes(FileInfo fileInfo, FileAttributes attributes, LoggerBase logger = null)
        {
            try
            {
                fileInfo.Attributes = attributes;
                return true;
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, LogLevel.Debug, fileNotFoundEx, string.Format("Can't change dirAttributes for file \"{0}\" to \"{1}\"", fileInfo, attributes));
            }
            catch (Exception attrException)
            {
                ConditionalLogging.LogExceptionOrThrowIt(logger, LogLevel.Error, attrException, string.Format("Can't change dirAttributes for file \"{0}\" to \"{1}\"", fileInfo, attributes));
            }
            return false;
        }

        public static string ChangeDirectoryForFile(string fullFileName, string newDirectory)
        {
            var fileName = Path.GetFileName(fullFileName);
            var destFileName = Path.Combine(newDirectory, fileName);
            return destFileName;
        }

        public static void MoveFileToDir(string sourceFileName, string destDirName)
        {
            //const string METH_NAME = "MoveFileToDir: ";
            var destFileName = ChangeDirectoryForFile(sourceFileName, destDirName);

            //Logger.Current.Log(LogLevel.Debug, CLASS_NAME + METH_NAME + "sourceFileName: \"{0}\"; destFileName: \"{1}\";", sourceFileName, destFileName);

            CreateDirectoryIfItDoesntExistForFile(destFileName, () => File.Move(sourceFileName, destFileName));
        }

        public static void MoveFilesToDir(IEnumerable<string> sourceFileNames, string destDirName)
        {
            foreach (var sourceFileName in sourceFileNames)
            {
                MoveFileToDir(sourceFileName, destDirName);
            }
        }

        public static string MoveFileWithDirectoryStructure(string sourceFileName, string destDirName, string remainStructureDir)
        {
            var sourceDir = Path.GetDirectoryName(sourceFileName);

            if (!sourceFileName.StartsWith(remainStructureDir))
            {
                throw new Exception(string.Format("sourceFileName directory should start with remainStructureDir. sourceFileName directory: \"{0}\"; remainStructureDir: \"{1}\"", sourceDir, remainStructureDir));
            }
            
            var structureDir = sourceDir.Substring(remainStructureDir.Length);
            var destDirNameWithStructureDir = Path.Combine(destDirName, structureDir);

            MoveFileToDir(sourceFileName, destDirNameWithStructureDir);

            return sourceDir;
        }

        public static bool IsDirectoryEmpty(string dirName)
        {
            var files = Directory.GetFiles(dirName);
            if (files.Length > 0)
                return false;

            var dirs = Directory.GetDirectories(dirName);
            if (dirs.Length > 0)
                return false;

            return true;
        }
    }
}
