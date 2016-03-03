using System;
using System.Collections.Generic;
using DotNetLog.ILogEntries;
using DotNetLog.ILoggers;

namespace DotNetLog.Loggers
{
    public class InMemoryLogger : ILogger
    {
        private static List<ILogEntry> LogEntries { get; } = new List<ILogEntry>();

        public void Log(ILogEntry logEntry)
        {
            LogEntries.Add(logEntry);
        }

        public ICollection<ILogEntry> GetLogsFromPeriod(TimeSpan timeSpan)
        {
            return LogEntries.FindAll(x => x.LogTime - DateTime.Now <= timeSpan);
        }

        public ICollection<ILogEntry> GetLogsByTypeFromPeriod(LogType logType, TimeSpan timeSpan)
        {
            return LogEntries
                .FindAll(x => x.LogType == logType)
                .FindAll(x => x.LogTime - DateTime.Now <= timeSpan);
        }

        public ICollection<ILogEntry> GetAllLogs()
        {
            return LogEntries;
        }
    }
}
