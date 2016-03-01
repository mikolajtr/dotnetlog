using System;
using DotNetLog.ILogEntries;

namespace DotNetLog.LogEntries
{
    public class LogInfo : LogEntry, ILogInfo
    {
        public LogInfo(string message) : base(message)
        {
            LogType = LogType.Info;
        }

        public LogInfo(string message, Exception exception) : base(message, exception)
        {
            LogType = LogType.Info;
        }
    }
}
