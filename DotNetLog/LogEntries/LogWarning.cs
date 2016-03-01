using System;
using DotNetLog.ILogEntries;

namespace DotNetLog.LogEntries
{
    public class LogWarning : LogEntry, ILogWarning
    {
        public LogWarning(string message) : base(message)
        {
            LogType = LogType.Warning;
        }

        public LogWarning(string message, Exception exception) : base(message, exception)
        {
            LogType = LogType.Warning;
        }
    }
}
