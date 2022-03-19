namespace AG.PathStringOperations
{
    public enum ActionWithFileAfterBackupResult
    {
        NoActionPerformed,
        ActionWithOriginalFilePerformed,
        RestoredBackupToFileAndActionPerformed,
        NoFileAndBackupFileButActionIsPerformed,
        NoFileAndBackupFileAndActionIsNotPerformed
    }
}
