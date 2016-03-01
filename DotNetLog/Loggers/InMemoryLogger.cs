using System;
using System.Collections.Generic;
using DotNetLog.ILogEntries;
using DotNetLog.ILoggers;

namespace DotNetLog.Loggers
{
    public class InMemoryLogger : ILogger
    {
        public static List<ILogEntry> LogEntries { get; } = new List<ILogEntry>();

        public void Log(ILogEntry logEntry)
        {
            LogEntries.Add(logEntry);
        }

        public ICollection<ILogEntry> GetLogsFromPeriod(TimeSpan timeSpan)
        {
            return LogEntries.FindAll(x => x.LogTime - DateTime.Now <= timeSpan);
        }

        public ICollection<ILogEntry> GetLogsByType(Type logType, TimeSpan timeSpan)
        {
            return LogEntries
                .FindAll(x => x.GetType() == logType)
                .FindAll(x => x.LogTime - DateTime.Now <= timeSpan);
        }

        public ICollection<ILogEntry> GetAllLogs()
        {
            return LogEntries;
        }
    }
}
