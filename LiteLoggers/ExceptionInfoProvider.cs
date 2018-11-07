using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AG
{
    public static class ExceptionInfoProvider
    {
        public static string GetExceptionInfo(Exception e)
        {
            return GetExceptionInfo(e, "\t", true);
        }
        public static string GetExceptionInfo(Exception e, bool includeStackTrace, bool includeExceptionType = true, bool includeSource = true, bool includeTargetSite = true)
        {
            return GetExceptionInfo(e, "\t", includeStackTrace, includeExceptionType, includeSource, includeTargetSite);
        }

        public static string GetExceptionInfo(Exception e, string indent, bool includeStackTrace = true, bool includeExceptionType = true, bool includeSource = true, bool includeTargetSite = true)
        {
            StringBuilder sb = new StringBuilder();
            if (includeExceptionType)
            {
                sb.Append(e.GetType());
                sb.Append(": ");
            }

            sb.AppendLine(e.Message);

            if (includeStackTrace)
            {
                sb.AppendLine(indent + "StackTrace:" + e.StackTrace);
            }
            if (!string.IsNullOrEmpty(e.HelpLink))
                sb.AppendLine(indent + "HelpLink: " + e.HelpLink);
            if (includeSource && !string.IsNullOrEmpty(e.Source))
                sb.AppendLine(indent + "Source: " + e.Source);
            if (includeTargetSite && e.TargetSite != null)
                sb.AppendLine(indent + "TargetSite: " + e.TargetSite);
            IDictionary exceptionData = e.Data;
            if (exceptionData.Count > 0)
            {
                ICollection keys = exceptionData.Keys;
                sb.AppendLine(indent + "Data:");
                foreach (var key in keys)
                {
                    sb.AppendFormat(indent + "\t{0}: {1}", key, exceptionData[key]);
                }
            }
            Exception innerException = e.InnerException;
            if (innerException != null)
                sb.AppendLine(indent + "InnerException: " + GetExceptionInfo(innerException, indent + indent, includeStackTrace, includeExceptionType, includeSource, includeTargetSite));

            return sb.ToString();
        }
    }
}
