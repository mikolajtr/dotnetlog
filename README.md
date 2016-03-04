# dotnetlog
Simple logging system for .NET apps.

There are two basic interfaces for entire mechanism:
 - `ILogEntry` - interface that represents single log. Log entry must contain: 
   + `string Message` - field for log message;
   + `LogType LogType` - type of log. It can be one of the chosen types from `LogTime` enum: `Info`, `Warning` or `Error`;
   + `DateTime LogTime` - log entry date and time creation;
   + `string LoggedException` - type and text of exception(if it was thrown);
 - `ILogger` - interface that represents log source(file, database etc.) Logger must contain:
   + `void Log(ILogEntry logEntry)` - method takes `ILogEntry` object and save it in log source(log file, database etc.);
   + `ICollection<ILogEntry> GetLogsFromPeriod(TimeSpan timeSpan)` - returns collection of logs from given `timeSpan`;
   + `ICollection<ILogEntry> GetLogsByTypeFromPeriod(LogType logType, TimeSpan timeSpan)` - returns collection of logs from given `timeSpan` and with chosen `logType`
   + `ICollection<ILogEntry> GetAllLogs()` - returns all logs.

There is `LogEntry` class that implements `ILogEntry`. `LogEntry` have three overloaded contstructors:
 - `LogEntry(string record)` - creates `LogEntry` from `record` - single log from log source;
 - `LogEntry(string message, LogType logType)` - creates `LogEntry` of the given type `logType` with `message`;
 - `LogEntry(string message, LogType logType, Exception exception)` - like the previous one; in addition saves exception type and message of `exception` argument. REMEMBER: do not fortget to rethrow exception after saving it in log.

`LogEntry` need two settings in `App.config` file in your application:
 - `LogPattern` - the pattern for string representation for single log. Default pattern is "`{0} | {1} | {2} | {3}`", where:
   + 0 - `LogTime`;
   + 1 - `LogType`;
   + 2 - `Message`;
   + 3 - `Exception`;
 - `LogRegexPattern` - the regular expression pattern of log; needed to recreate `LogEntry` object from its text form. Default regex pattern is "`(\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d) \| (\w*) \| (.*) \| (.*)`"

For saving logs, there are two example implementations of `ILogger`:
 - `FileLogger` - saves given logs in file. Log file path should be saved in your application `App.config` file, under `LogFilePath` key. Default log file `log.txt` will be created in your app main directory;
 - `InMemoryLogger` - saves logs in static list stored in application memory. When program is closed, logs are destroyed. Do NOT use this logger to things other than debugging!
Both `FileLogger` and `InMemoryLogger` are singletons - there is only one instance of logger for application. To get to instance, you have use `Instance` property of logger.

Example of using log system with `FileLogger` is within Sample console app in this solution. Run it or check example below:
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
                ILogger logger = FileLogger.Instance; // Get instance of logger. Logger is singleton, you can't create it directly.
    
                logger.Log(new LogEntry("Sample log.", LogType.Info)); // Log new info log.
                logger.Log(new LogEntry("Sample log error.", LogType.Error, new Exception("Error!"))); // Log new error log and log exception. Do not forget to rethrow it in try/catch block!
                logger.Log(new LogEntry("Sample log warning", LogType.Warning)); // Log new warning log.
    
                Console.WriteLine("Logs created. Check your log file(log.txt default).");
                Console.WriteLine("Press ENTER to close.");
                Console.ReadKey();
            }
        }
    }

Your log.txt file should looks like:

    2016-03-04 11:13:20 | Info | Sample log. | 
    2016-03-04 11:13:20 | Error | Sample log error. | System.Exception: Error!
    2016-03-04 11:13:20 | Warning | Sample log warning | 
