using System;
using DotNetLog.ILogEntries;

namespace DotNetLog.LogEntries
{
    public class LogEntry : ILogEntry
    {
        public string Message { get; set; }

        public LogType LogType { get; set; }

        public DateTime LogTime { get; set; }

        public string LoggedException { get; set; }

        public LogEntry(string message)
        {
            LogTime = DateTime.Now;
            Message = message;
        }

        public LogEntry(string message, Exception exception)
        {
            LogTime = DateTime.Now;
            Message = message;
            LoggedException = exception.ToString();
        }

        public override string ToString()
        {
            return $"{LogTime} | {LogType} | {Message} | {LoggedException}";
        }
    }
}
