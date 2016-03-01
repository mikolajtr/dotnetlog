using System;
using DotNetLog.LogEntries;

namespace DotNetLog.ILogEntries
{
    public interface ILogEntry
    {
        string Message { get; set; }

        LogType LogType { get; set; }

        DateTime LogTime { get; set; }

        string LoggedException { get; set; }
    }
}
