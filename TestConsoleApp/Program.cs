using AG.AssemblyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AG.Loggers;
using AG.PathStringOperations;
using System.IO;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\1\2\..\..\..\3.txt";
            path = Path.GetFullPath(path);

            Loggers logger = new Loggers(new FileLogger(@"D:\Recycle\!delete\ProjectsGarbage\test_log.log", LogLevel.Debug),
                new ConsoleLogger(LogLevel.Debug));
            //logger.WriteLogMode = FileLogger.WriteLogOptions.LockAndFlushAfterEveryWrite;
            //logger.WriteLogMode = FileLogger.WriteLogOptions.Lock;
            //logger.TypeWord = new LoggerBase.LogTypeWord("I", "W", "E", "D");
            //logger.FormatMessage = (msgLogLevel, message) => { return string.Format("{2} {0}: {1}", msgLogLevel, message, DateTime.Now.ToString(@"HH\:mm\:ss")); };
            logger.Log(LogLevel.Info, "Test infooo");
            logger.Log(LogLevel.Info, "2");
            logger.Log(LogLevel.Info, "3");
            logger.Log(LogLevel.Info, "4");
            logger.Log(LogLevel.Info, "5");
            logger.Log(LogLevel.Info, "6");
            //try
            //{
            //    int i = TestMethod2(0.321F);
            //}
            //catch (Exception ex)
            //{
            //    logger.Log(LogLevel.Error, ex);
            //}
            //Console.WriteLine(ProgramInfo.FullProgramName);
            //Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            //asm = Assembly.Get
            //ProgramInfo.SetProgramMainAssembly(asm);
            //Console.WriteLine(ExtendedPath.GetRightFileNameFromString(ProgramInfo.Name));
            Console.WriteLine("end.");
            Console.ReadKey();
        }

        static int TestMethod(float f)
        {
            throw new ApplicationException("Льолька");
        }

        static int TestMethod2(float f)
        {
            try
            {
                return TestMethod(f);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("inv", e);
            }
        }
    }
}
