using System;
using DotNetLog;
using DotNetLog.ILoggers;
using DotNetLog.Loggers;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new FileLogger();

            var logs = logger.GetLogsFromPeriod(new TimeSpan(0, 1, 0, 0));

            int j = 0;
            foreach (var logEntry in logs)
            {
                Console.WriteLine($"Log no:{++j}; Date: {logEntry.LogTime}, Exception: {logEntry.LoggedException}");
            }
            Console.ReadKey();
        }
    }
}
