using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace AG.PathStringOperations
{
    public static class ExtendedPath
    {
        //todo: make it crossplatform
        public static int MAX_PATH_LENGTH = 259;

        //public static string ApplicationExecutablePath { get { return System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\"; } }

        public static string ApplicationDirectory
        {
            get
            {
                string currentDir = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                if (string.IsNullOrEmpty(currentDir))
                    currentDir = Application.StartupPath;
                if (!currentDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    currentDir = currentDir + Path.DirectorySeparatorChar;
                return currentDir;
            }
        }

        public static string GetRelativePath(string relativelyThisPath, string absoluteFullPath)
        {
            if (absoluteFullPath == null || !absoluteFullPath.StartsWith(relativelyThisPath))
                return absoluteFullPath;
            string relativePath = absoluteFullPath.Substring(relativelyThisPath.Length);
            if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                relativePath = relativePath.Substring(1);
            return relativePath;
        }

        public static string GetRootedPath(string notRootedPath, string rootedCurrentDirectory)
        {
            if (!Path.IsPathRooted(notRootedPath))
            {
                string rootedPath = Path.Combine(rootedCurrentDirectory, notRootedPath);
                return Path.GetFullPath(rootedPath);
            }
            return notRootedPath;
        }

        public static string GetRightFileNameFromString(string stringWithInvalidChars)
        {
            string logFileName = ReplaceAllCharsInString(stringWithInvalidChars, Path.GetInvalidFileNameChars(), '_');
            return logFileName;
        }

        private static string ReplaceAllCharsInString(string inputString, char[] replacedChars, char replacingChar)
        {
            int strLength = inputString.Length;
            int charsLength = replacedChars.Length;
            StringBuilder sb = new StringBuilder(strLength);
            for (int i = 0; i < strLength; i++)
            {
                char currentChar = inputString[i];
                for (int j = 0; j < charsLength; j++)
                {
                    if (currentChar == replacedChars[j])
                    {
                        currentChar = replacingChar;
                        break;
                    }
                }
                sb.Append(currentChar);
            }
            return sb.ToString();
        }

        public static string GetPathWithoutLast(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("path can't be null or empty");
            }

            int lastIndex = path.Length - 1;
            int startPos;
            if (path[lastIndex] == Path.DirectorySeparatorChar)
            {
                startPos = lastIndex - 1;
                if (startPos == 0)
                {
                    return path;
                }
            }
            else
            {
                startPos = lastIndex;
            }

            int slashPos = path.LastIndexOf(Path.DirectorySeparatorChar, startPos);
            if (slashPos == -1)
                return path;
            else
                return path.Substring(0, slashPos);
        }

        public static string GetPathWithoutExtension(string path)
        {
            int extPos = path.Length - 1;
            for (; extPos > 0; extPos--)
            {
                if (path[extPos] == Path.DirectorySeparatorChar)
                    return path;
                else if (path[extPos] == '.')
                    break;
            }
            if (extPos <= 0)
                return path;
            else
                return path.Substring(0, extPos);
        }

        public static string GetIncrementedPath(string path, string pattern)
        {
            return GetIncrementedPath(path, pattern, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern">Pattern which means number part. Example: ({0})</param>
        /// <param name="startNumber"></param>
        /// <returns></returns>
        public static string GetIncrementedPath(string path, string pattern, int startNumber, bool freePathOnly = false)
        {
            string pathWithoutExt = GetPathWithoutExtension(path);
            string pathExt = Path.GetExtension(path);

            Regex regex = new Regex(@"\{[^{]+\}", RegexOptions.IgnoreCase | RegexOptions.RightToLeft | RegexOptions.Compiled);
            Match match = regex.Match(pattern);
            if (!match.Success)
                throw new ArgumentException("The pattern must include an index place-holder as \"{0}\"", "pattern");
            string prePattern;
            int posPrePattern = pattern.LastIndexOf(match.Value);
            if (posPrePattern == 0)
                prePattern = "";
            else
                prePattern = pattern.Substring(0, posPrePattern);

            string postPattern;
            int patternLength = pattern.Length;
            int posPostPattern = posPrePattern + match.Value.Length;
            if (posPostPattern >= patternLength)
                postPattern = "";
            else
                postPattern = pattern.Substring(posPostPattern, patternLength - posPostPattern);

            string regexPattern = Regex.Escape(prePattern) + "[0-9]+" + Regex.Escape(postPattern);
            regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.RightToLeft | RegexOptions.Compiled);
            match = regex.Match(pathWithoutExt);

            string matchValue = match.Value;
            bool isNumberInNameExists = match.Success;
            int num = 0;//~!!!!1
            if (isNumberInNameExists && !pathWithoutExt.EndsWith(matchValue))
            {
                isNumberInNameExists = false;
            }
            else
            {
                string strNum = matchValue;
                if (!string.IsNullOrEmpty(prePattern))
                    strNum = matchValue.Replace(prePattern, "");
                if (!string.IsNullOrEmpty(postPattern))
                    strNum = strNum.Replace(postPattern, "");
                isNumberInNameExists = int.TryParse(strNum, out num);

            }


            bool fileWithSuchPathExists = false;
            bool pathMustBeIncremented = false;
            do
            {
                if (!pathMustBeIncremented && !isNumberInNameExists)
                {
                    pathWithoutExt += string.Format(pattern, startNumber);
                }
                else
                {
                    num++;
                    pathWithoutExt = pathWithoutExt.Remove(pathWithoutExt.Length - matchValue.Length);
                    pathWithoutExt += string.Format(pattern, num);
                }

                if (freePathOnly)
                {
                    var finalPath = pathWithoutExt + pathExt;
                    fileWithSuchPathExists = File.Exists(finalPath);
                    if (fileWithSuchPathExists)
                    {
                        pathMustBeIncremented = true;
                    }
                }
            } while (freePathOnly && fileWithSuchPathExists);

            return pathWithoutExt + pathExt;
        }

        public static string GetLastPath(string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return Path.GetFileName(path.Substring(0, path.Length - 2));
            else
                return Path.GetFileName(path);
        }

        public static string GetLastExistingDirectory(string path)
        {
            string prevPath = null;
            while (!Directory.Exists(path))
            {
                if (path == prevPath) //if path already is rooted up directory
                {
                    return string.Empty;
                }
                prevPath = path;
                path = GetPathWithoutLast(path);
            }
            return path;
        }

        public static bool ContainsInvalidFileNameChars(string path)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return path.Any(c => invalidChars.Contains(c));
        }

        public static void TakeNameFromAnotherFile(string anotherFileName, string fileToRename)
        {
            var anotherFileNameShort = Path.GetFileName(anotherFileName);

            var fileToRenameDir = Path.GetDirectoryName(fileToRename);

            var newFileName = Path.Combine(fileToRenameDir, anotherFileNameShort);

            File.Move(fileToRename, newFileName);
        }

        public static bool IsDirectory(string fileOrDirectoryPath)
        {
            var pathAttributes = File.GetAttributes(fileOrDirectoryPath);
            return (pathAttributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static PathType GetPathType(string fileOrDirectoryPath)
        {
            try
            {
                if (IsDirectory(fileOrDirectoryPath))
                {
                    return PathType.Directory;
                }
                else
                {
                    return PathType.File;
                }
            }
            catch (FileNotFoundException)
            {
                return PathType.Absent;
            }
            catch (DirectoryNotFoundException)
            {
                return PathType.Absent;
            }
        }

        public static string ChangeFileName(string fileName, FileNameOperation operarion)
        {
            var fileNameWithoutExtension = GetPathWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var changedFileName = operarion(fileNameWithoutExtension, extension);
            return changedFileName;
        }
    }
}
