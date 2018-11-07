using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using AG.AssemblyInfo;
using AG.PathStringOperations.PathEncoders;

namespace AG.PathStringOperations
{
    public class ApplicationFolders
    {
        private string _localDir;
        private string _roamingDir;
        private string _appDir;

        public ApplicationFolders(string companyNameDir, string appDataNameDir = null)
        {
            _appDir = ExtendedPath.ApplicationDirectory;

            string companyAndEncodedAppDataFolderName = GetCompanyAndEncodedAppDataFolderName(companyNameDir, appDataNameDir);
            _localDir = GetLocalDir(companyAndEncodedAppDataFolderName);
            _roamingDir = GetRoamingDir(companyAndEncodedAppDataFolderName);
        }

        //public ApplicationFolders(string commonDirForEverything)
        //    : this(commonDirForEverything, commonDirForEverything, commonDirForEverything)
        //{
        //}

        public ApplicationFolders(string localDir, string roamingDir, string appDir)
        {
            _localDir = localDir;
            _roamingDir = roamingDir;
            _appDir = appDir;
        }

        public string LocalDir
        {
            get { return _localDir; }
            set { _localDir = value; }
        }

        public string RoamingDir
        {
            get { return _roamingDir; }
            set { _roamingDir = value; }
        }

        public string AppDir
        {
            get { return _appDir; }
            set { _appDir = value; }
        }

        public static string DefaultCompanyDir
        {
            get
            {
                PathEncoderByChar pathEncoderByChar = new PathEncoderByChar('_');
                string encodedCompanyName = pathEncoderByChar.EncodeFileName(ProgramInfo.Company);
                return encodedCompanyName;
            }
        }

        public static string GetLocalDir(string companyAndEncodedAppDataFolderName = null)
        {
            if(companyAndEncodedAppDataFolderName == null)
            {
                companyAndEncodedAppDataFolderName = GetCompanyAndEncodedAppDataFolderName(DefaultCompanyDir);
            }
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyAndEncodedAppDataFolderName);
        }

        public static string GetRoamingDir(string companyAndEncodedAppDataFolderName = null)
        {
            if (companyAndEncodedAppDataFolderName == null)
            {
                companyAndEncodedAppDataFolderName = GetCompanyAndEncodedAppDataFolderName(DefaultCompanyDir);
            }
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), companyAndEncodedAppDataFolderName);
        }

        public static string GetCompanyAndEncodedAppDataFolderName(string companyNameDir, string appDataNameDir = null)
        {
            if (string.IsNullOrEmpty(appDataNameDir))
            {
                appDataNameDir = ProgramInfo.Name;
            }

            PathEncoderByChar pathEncoderByChar = new PathEncoderByChar('_');
            string encodedAppDataFolderName = pathEncoderByChar.EncodeFileName(appDataNameDir);
            string companyAndEncodedAppDataFolderName = Path.Combine(companyNameDir, encodedAppDataFolderName);
            return companyAndEncodedAppDataFolderName;
        }

        public override string ToString()
        {
            return string.Format("App: \"{0}\"\r\nRoaming: \"{1}\"\r\nLocal: \"{2}\"", AppDir, RoamingDir, LocalDir);
        }
    }
}
