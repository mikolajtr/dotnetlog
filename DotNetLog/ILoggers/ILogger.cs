using System;
using System.Collections.Generic;
using DotNetLog.ILogEntries;

namespace DotNetLog.ILoggers
{
    public interface ILogger
    {
        void Log(ILogEntry logEntry);
        ICollection<ILogEntry> GetLogsFromPeriod(TimeSpan timeSpan);
        ICollection<ILogEntry> GetLogsByType(Type logType, TimeSpan timeSpan);
        ICollection<ILogEntry> GetAllLogs();
    }
}
