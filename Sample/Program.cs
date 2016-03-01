using System;
using DotNetLog.LogEntries;
using DotNetLog.Loggers;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            InMemoryLogger logger = new InMemoryLogger();
            logger.Log(new LogInfo("pierwszy log"));
            logger.Log(new LogWarning("drugi log"));
            logger.Log(new LogInfo("trzeci log", new NullReferenceException()));
            logger.Log(new LogError("czwarty log", new ArgumentNullException()));

            foreach (var logEntry in InMemoryLogger.LogEntries)
            {
                Console.WriteLine(logEntry);
            }

            Console.ReadKey();
        }
    }
}
