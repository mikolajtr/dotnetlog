using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using DotNetLog.Exceptions;
using DotNetLog.ILogEntries;
using DotNetLog.ILoggers;
using DotNetLog.LogEntries;

namespace DotNetLog.Loggers
{
    public sealed class FileLogger : ILogger
    {
        private static volatile FileLogger _instance;
        private static readonly object SyncRoot = new Object();
        private readonly Dictionary<Type, string> _readLogExceptions;
        private readonly Dictionary<Type, string> _writeLogExceptions;
        private readonly string _filePath;

        private List<ILogEntry> LogEntries { get { return (List<ILogEntry>)GetAllLogs(); } }

        public static FileLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new FileLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        private FileLogger()
        {
            _writeLogExceptions = new Dictionary<Type, string>
            {
                { typeof(UnauthorizedAccessException), "You don't have permission to create log file." },
                { typeof(ArgumentException), "Invalid log file path." },
                { typeof(ArgumentNullException), "Log file path is empty." },
                { typeof(PathTooLongException), "Log file path is too long." },
                { typeof(DirectoryNotFoundException), "Directory with log file cannot be found." },
                { typeof(NotSupportedException), "I/O operation not supported." },
                { typeof(IOException), "I/O operation error." },
                { typeof(ObjectDisposedException), "Log file object was already disposed." },
                { typeof(Exception), "Unknown error while writing log to file." }
            };
            _readLogExceptions = new Dictionary<Type, string>
            {
                { typeof(ArgumentException), "Invalid log file path." },
                { typeof(ArgumentNullException), "Log file path is empty." },
                { typeof(DirectoryNotFoundException), "Directory in log file path cannot be found." },
                { typeof(FileNotFoundException), "Log file cannot be found." },
                { typeof(IOException), "I/O operation error." },
                { typeof(Exception), "Unknown error while reading log from file." }
            };
            _filePath = ConfigurationManager.AppSettings["LogFilePath"];
            if (_filePath == null)
            {
                _filePath = "log.txt";
            }
        }

        public void Log(ILogEntry logEntry)
        {
            try
            {
                using (var logFile = File.AppendText(_filePath))
                {
                    logFile.WriteLine(logEntry.ToString());
                }
            }
            catch (Exception e)
            {
                HandleExceptions(e, _writeLogExceptions);
            }
        }

        public ICollection<ILogEntry> GetLogsFromPeriod(TimeSpan timeSpan)
        {
            return LogEntries.FindAll(x => (DateTime.Now - x.LogTime) <= timeSpan);
        }

        public ICollection<ILogEntry> GetLogsByTypeFromPeriod(LogType logType, TimeSpan timeSpan)
        {
            return LogEntries
                .FindAll(x => x.LogType == logType)
                .FindAll(x => (DateTime.Now - x.LogTime) <= timeSpan);
        }

        public ICollection<ILogEntry> GetAllLogs()
        {
            List<ILogEntry> logEntries= new List<ILogEntry>();
            try
            {
                using (var logFile = new StreamReader(_filePath))
                {
                    string line;
                    while ((line = logFile.ReadLine()) != null)
                    {
                        logEntries.Add(new LogEntry(line));
                    }
                }
            }
            catch (Exception e)
            {
                HandleExceptions(e, _readLogExceptions);
            }

            return logEntries;
        }

        private void HandleExceptions(Exception e, Dictionary<Type, string> exceptions)
        {
            foreach (var logException in exceptions)
            {
                if (e.GetType() == logException.Key)
                {
                    throw new LoggingException(logException.Value, e);
                }
            }
        }
    }
}
