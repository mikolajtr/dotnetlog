using System;
using DotNetLog.ILogEntries;

namespace DotNetLog.LogEntries
{
    public class LogError : LogEntry, ILogError
    {
        public LogError(string message) : base(message)
        {
            LogType = LogType.Error;
        }

        public LogError(string message, Exception exception) : base(message, exception)
        {
            LogType = LogType.Error;
        }
    }
}
