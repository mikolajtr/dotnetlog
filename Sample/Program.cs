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
            ILogger logger = FileLogger.GetLoggerInstance(); // Get instance of logger. Logger is singleton, you can't create it directly.

            logger.Log(new LogEntry("Sample log.", LogType.Info)); // Log new info log.
            logger.Log(new LogEntry("Sample log error.", LogType.Error, new Exception("Error!"))); // Log new error log and log exception. Do not forget to rethrow it in try/catch block!
            logger.Log(new LogEntry("Sample log warning", LogType.Warning)); // Log new warning log.

            Console.WriteLine("Logs created. Check your log file(log.txt default).");
            Console.WriteLine("Press ENTER to close.");
            Console.ReadKey();
        }
    }
}
