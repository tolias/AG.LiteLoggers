using System;
using System.Collections.Generic;
using System.Text;

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
