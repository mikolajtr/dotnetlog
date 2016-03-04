using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using DotNetLog.ILogEntries;
using DotNetLog.ILoggers;
using DotNetLog.LogEntries;

namespace DotNetLog.Loggers
{
    public sealed class FileLogger : ILogger
    {
        private static volatile FileLogger _instance;
        private static readonly object SyncRoot = new Object();

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
            _filePath = ConfigurationManager.AppSettings["LogFilePath"];
            if (_filePath == null)
            {
                _filePath = "log.txt";
            }
        }

        private List<ILogEntry> LogEntries { get { return (List<ILogEntry>) GetAllLogs(); } } 
         
        private readonly string _filePath;

        public void Log(ILogEntry logEntry)
        {
            using (var logFile = File.AppendText(_filePath))
            {
                logFile.WriteLine(logEntry.ToString());
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
            using (var logFile = new StreamReader(_filePath))
            {
                string line;
                while ((line = logFile.ReadLine()) != null)
                {
                    logEntries.Add(new LogEntry(line));
                }
            }

            return logEntries;
        }
    }
}
