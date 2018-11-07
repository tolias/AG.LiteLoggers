using System;
using System.IO;

namespace AG.PathStringOperations
{
    public static class OperationsWithBackingUpFile
    {
        public const string DEFAULT_BACKUP_FILE_NAME_PATTERN = "{0}.tmp";

        /// <summary>
        /// Performs an action that is specified in <paramref name="actionWithFile"/> doing backup before it
        /// </summary>
        /// <param name="fileName">The final name of file</param>
        /// <param name="backupFileNamePattern">The temporary backup file name. It can be a pattern where "{0}" is <paramref name="fileName"/></param>
        /// <param name="actionWithFile">The action which will be performed. 'string' parameter is a backup file name</param>
        public static void ActionWithFileWithBackingUp(string fileName, Action<string> actionWithFile, string backupFileNamePattern = DEFAULT_BACKUP_FILE_NAME_PATTERN)
        {
            string backupFileName = string.Format(backupFileNamePattern, fileName);
            FileDirectoryManager.CreateDirectoryIfItDoesntExistForFile(backupFileName, () => actionWithFile(backupFileName));
            File.Delete(fileName);
            File.Move(backupFileName, fileName);
        }

        /// <summary>
        /// If <paramref name="fileName"/> exists: performs <paramref name="actionWithFile"/>;
        /// Otherwise: checks if <paramref name="backupFileNamePattern"/> exists.
        /// If yes: moves it to <paramref name="fileName"/> and performs <paramref name="actionWithFile"/>;
        /// If no: just performs <paramref name="actionWithFile"/> (even it doesn't exist);
        /// </summary>
        /// <param name="fileName">The original name of file</param>
        /// <param name="backupFileNamePattern">The temporary backup file name. It can be a pattern where "{0}" is <paramref name="fileName"/>
        /// <param name="actionWithFile">The action which will be performed. 'string' parameter is a file name</param>
        /// <returns>Info about performed action</returns>
        public static ActionWithFileAfterBackupResult ActionWithFileAfterBackUp(string fileName, Action<string> actionWithFile,
            string backupFileNamePattern = DEFAULT_BACKUP_FILE_NAME_PATTERN, bool performActionIfEvenFileAndBackupFileDontExist = false)
        {
            ActionWithFileAfterBackupResult actionWithFileAfterBackupResult;
            if (File.Exists(fileName))
            {
                actionWithFileAfterBackupResult = ActionWithFileAfterBackupResult.ActionWithOriginalFilePerformed;
            }
            else
            {
                string backupFileName = string.Format(backupFileNamePattern, fileName);
                if (File.Exists(backupFileName))
                {
                    actionWithFileAfterBackupResult = ActionWithFileAfterBackupResult.RestoredBackupToFileAndActionPerformed;
                    File.Move(backupFileName, fileName);
                }
                else if (!performActionIfEvenFileAndBackupFileDontExist)
                {
                    actionWithFileAfterBackupResult = ActionWithFileAfterBackupResult.NoFileAndBackupFileAndActionIsNotPerformed;
                    return actionWithFileAfterBackupResult;
                }
                else
                {
                    actionWithFileAfterBackupResult = ActionWithFileAfterBackupResult.NoFileAndBackupFileButActionIsPerformed;
                }
            }
            actionWithFile(fileName);
            return actionWithFileAfterBackupResult;
        }
    }
}
