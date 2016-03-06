using System;
using System.Collections.Generic;
using DotNetLog.ILogEntries;
using DotNetLog.ILoggers;

namespace DotNetLog.Loggers
{
    public sealed class InMemoryLogger : ILogger
    {
        private static volatile InMemoryLogger _instance;
        private static readonly object SyncRoot = new Object();

        private static List<ILogEntry> LogEntries { get; } = new List<ILogEntry>();

        public static InMemoryLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new InMemoryLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        private InMemoryLogger()
        {
        }

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

        public ICollection<ILogEntry> GetLogsByType(LogType logType)
        {
            return LogEntries.FindAll(x => x.LogType == logType);
        }

        public ICollection<ILogEntry> GetAllLogs()
        {
            return LogEntries;
        }
    }
}
