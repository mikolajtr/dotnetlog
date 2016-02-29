using System;

namespace DotNetLog.LogEntries
{
    public interface ILogEntry
    {
        DateTime LogTime { get; set; }
        Exception LoggedException { get; set; }
    }
}
