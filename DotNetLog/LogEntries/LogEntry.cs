using System;
using System.Configuration;
using DotNetLog.Exceptions;
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
            LoggedException = string.Empty;
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
            string[] fields = record.Split(_separator);
            DateTime logTime;
            string message;
            string loggedException;
            LogType logType;

            try
            {
                logTime = DateTime.Parse(fields[0].Trim());
            }
            catch (FormatException e)
            {
                throw new LoggingException("Incorrect DateTime format.", e);
            }
            catch (Exception e)
            {
                throw new LoggingException("Incorrect log format.", e);
            }

            try
            {
                logType = (LogType) Enum.Parse(typeof (LogType), fields[1].Trim());
            }
            catch (ArgumentNullException e)
            {
                throw new LoggingException("Incorrect log format.", e);
            }
            catch (Exception e)
            {
                throw new LoggingException("Incorrect log type.", e);
            }

            try
            {
                message = fields[2].Trim();
                loggedException = fields[3].Trim();
            }
            catch (Exception e)
            {
                throw new LoggingException("Incorrect log format", e);
            }

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

        public override bool Equals(object obj)
        {
            LogEntry e = obj as LogEntry;
            if (e == null)
            {
                return false;
            }

            return (LogTime == e.LogTime) && (LogType == e.LogType) && (Message == e.Message) &&
                   (LoggedException == e.LoggedException);
        }

        public bool Equals(LogEntry e)
        {
            if (e == null)
            {
                return false;
            }

            return (LogTime == e.LogTime) && (LogType == e.LogType) && (Message == e.Message) &&
                   (LoggedException == e.LoggedException);
        }

        public override int GetHashCode()
        {
            return string.Concat(LogTime.ToString(), LogType, Message, LoggedException).GetHashCode();
        }
    }
}
