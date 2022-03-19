using System.Threading;

namespace AG
{
    public static class ExtensionMethods
    {
        public static bool SetNameIfNotNull(this Thread thread, string threadName)
        {
            if (thread.Name == null)
            {
                thread.Name = threadName;
                return true;
            }
            return false;
        }
    }
}
