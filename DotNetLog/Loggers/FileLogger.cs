using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using DotNetLog.ILogEntries;
using DotNetLog.ILoggers;
using DotNetLog.LogEntries;

namespace DotNetLog.Loggers
{
    public class FileLogger : ILogger
    {
        private static FileLogger _instance;

        public static FileLogger GetLoggerInstance()
        {
            if (_instance == null)
            {
                _instance = new FileLogger();
            }

            return _instance;
        }

        private FileLogger()
        {
            _filePath = ConfigurationManager.AppSettings["LogFilePath"];
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
