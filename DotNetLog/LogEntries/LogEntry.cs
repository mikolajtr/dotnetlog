using System;
using System.Configuration;
using DotNetLog.ILogEntries;

namespace DotNetLog.LogEntries
{
    public class LogEntry : ILogEntry
    {
        public string Message { get; set; }

        public LogType LogType { get; set; }

        public DateTime LogTime { get; set; }

        public string LoggedException { get; set; }

        private readonly char _separator;

        public LogEntry(string record)
        {
            _separator = GetLogSeparator();
            FromString(record);
        }

        public LogEntry(string message, LogType logType)
        {
            LogTime = DateTime.Now;
            Message = message;
            LogType = logType;
            _separator = GetLogSeparator();
        }

        public LogEntry(string message, LogType logType, Exception exception) : this(message, logType)
        {
            LoggedException = exception.ToString();
        }

        public override string ToString()
        {
            return $"{LogTime} {_separator} {LogType} {_separator} {Message} {_separator} {LoggedException}";
        }

        private void FromString(string record)
        {
            Console.WriteLine(record);
            string[] fields = record.Split(_separator);

            DateTime logTime = DateTime.Parse(fields[0].Trim());
            LogType logType = (LogType)Enum.Parse(typeof(LogType), fields[1].Trim());
            string message = fields[2].Trim();
            string loggedException = fields[3].Trim();

            LogTime = logTime;
            LogType = logType;
            Message = message;
            LoggedException = loggedException;
        }

        private char GetLogSeparator()
        {
            string separator = ConfigurationManager.AppSettings["LogFieldSeparator"];
            if (separator == null)
            {
                separator = "|";
            }
            return separator.Trim()[0];
        }
    }
}
