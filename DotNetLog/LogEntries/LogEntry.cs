using System;
using System.Configuration;
using System.Text.RegularExpressions;
using DotNetLog.ILogEntries;

namespace DotNetLog.LogEntries
{
    public class LogEntry : ILogEntry
    {
        public string Message { get; set; }

        public LogType LogType { get; set; }

        public DateTime LogTime { get; set; }

        public string LoggedException { get; set; }

        public LogEntry(string record)
        {
            FromString(record);
        }

        public LogEntry(string message, LogType logType)
        {
            LogTime = DateTime.Now;
            Message = message;
            LogType = logType;
        }

        public LogEntry(string message, LogType logType, Exception exception)
        {
            LogTime = DateTime.Now;
            Message = message;
            LogType = logType;
            LoggedException = exception.ToString();
        }

        public override string ToString()
        {
            return $"{LogTime} | {LogType} | {Message} | {LoggedException}";
        }

        private void FromString(string record)
        {
            string pattern = ConfigurationManager.AppSettings["LogPattern"];
            Match match = Regex.Match(record, pattern);
            DateTime logTime = DateTime.Parse(match.Groups[1].Value);
            LogType logType = (LogType)Enum.Parse(typeof(LogType), match.Groups[2].Value);
            string message = match.Groups[3].Value;
            string loggedException = match.Groups[4].Value;

            LogTime = logTime;
            LogType = logType;
            Message = message;
            LoggedException = loggedException;
        }
    }
}
