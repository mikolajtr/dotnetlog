using System;
using DotNetLog.ILogEntries;
using DotNetLog.LogEntries;
using NUnit.Framework;

namespace DotNetLog.Test
{
    [TestFixture]
    class LogEntryTests
    {
        private DateTime _rawDate;
        private string _testLogString;
        private ILogEntry _testLog;

        [SetUp]
        public void SetUp()
        {
            _rawDate = new DateTime(2016, 02, 15, 3, 50, 0);
            _testLogString = "2016-02-15 03:50:00 | Info | Sample log. | ";
            _testLog = new LogEntry("Sample log.", LogType.Info) { LogTime = _rawDate };
        }

        [Test]
        public void LogToStringTest()
        {
            Assert.AreEqual(_testLogString, _testLog.ToString());
        }

        [Test]
        public void LogFromStringTest()
        {
            var rawTextLog = new LogEntry(_testLogString);
            Assert.AreEqual(_testLog, rawTextLog);
        }
    }
}
