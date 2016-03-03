using System;
using DotNetLog.ILoggers;
using DotNetLog.Loggers;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = FileLogger.GetLoggerInstance();

            var logs = logger.GetLogsFromPeriod(new TimeSpan(0, 9, 10, 0));

            int j = 0;
            foreach (var logEntry in logs)
            {
                Console.WriteLine($"Log no:{++j}; Date: {logEntry.LogTime}, Exception: {logEntry.LoggedException}");
            }
            Console.ReadKey();
        }
    }
}
