using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace AG.AssemblyInfo
{
    public static class ProgramInfo
    {
        //private static Assembly programMainAssembly;
        private static string _name;
        private static string _version;
        private static string _configuration;
        private static string _company;

        public static string FullProgramName
        {
            get
            {
                return ProgramInfo.Name + " " + ProgramInfo.FullVersion;
            }
        }

        public static string Name
        {
            get
            {
                if (_name == null)
                {
                    Initialize();
                }
                return _name;
            }
        }

        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    Initialize();
                }
                return _version;
            }
        }

        public static string FullVersion
        {
            get
            {
                
                return Version + Configuration;
            }
        }

        public static string Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    Initialize();
                }
                return _configuration;
            }
        }

        public static string Company
        {
            get
            {
                if (_company == null)
                {
                    Initialize();
                }
                return _company;
            }
        }

        private static void Initialize()
        {
            Assembly programMainAssembly = Assembly.GetEntryAssembly();
            if (programMainAssembly == null)
                throw new InvalidOperationException("Can't initialize program info. Entry assembly is null...");
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(programMainAssembly.Location);
            _version = fvi.ProductVersion;
            _name = fvi.ProductName;
            _company = fvi.CompanyName;

            object[] attr = programMainAssembly.GetCustomAttributes(typeof(System.Reflection.AssemblyConfigurationAttribute), false);
            if (attr.Length > 0)
            {
                System.Reflection.AssemblyConfigurationAttribute aca = (System.Reflection.AssemblyConfigurationAttribute)attr[0];
                _configuration = aca.Configuration;
            }
        }

        public static string GetAssemblyVersion(Assembly assembly, string assemblyFileName)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assemblyFileName);
            object[] attr = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyConfigurationAttribute), false);
            System.Reflection.AssemblyConfigurationAttribute aca = (System.Reflection.AssemblyConfigurationAttribute)attr[0];
            return fvi.ProductVersion + aca.Configuration;
        }

        //public static void SetProgramMainAssembly(Assembly programMainAssembly)
        //{
        //    ProgramInfo.programMainAssembly = programMainAssembly;
        //}
    }
}
