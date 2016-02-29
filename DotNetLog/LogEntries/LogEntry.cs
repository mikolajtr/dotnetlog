using System;

namespace DotNetLog.LogEntries
{
    public class LogEntry : ILogEntry
    {
        public DateTime LogTime { get; set; }

        public Exception LoggedException { get; set; }
    }
}
