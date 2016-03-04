using System;
using DotNetLog;
using DotNetLog.ILoggers;
using DotNetLog.LogEntries;
using DotNetLog.Loggers;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = FileLogger.GetLoggerInstance();
            logger.Log(new LogEntry("Sample log of new formatting", LogType.Info));
            logger.Log(new LogEntry("Sample log error of new formatting", LogType.Error, new ArgumentException()));

            Console.ReadKey();
        }
    }
}
