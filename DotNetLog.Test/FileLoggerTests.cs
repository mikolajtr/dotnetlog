using System;
using System.Configuration;
using System.IO;
using System.Linq;
using DotNetLog.LogEntries;
using DotNetLog.Loggers;
using NUnit.Framework;

namespace DotNetLog.Test
{
    [TestFixture]
    public class FileLoggerTests
    {
        private FileLogger _logger;
        private LogEntry _log;
        private DateTime _cleanDate;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["LogFilePath"] = $"{Guid.NewGuid()}.txt"; // generate test log file with random GUID as file name
            _logger = FileLogger.Instance;
            var now = DateTime.Now;
            _cleanDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second); // mocking datetime to avoid trouble with miliseconds comparing
            _log = new LogEntry("test log", LogType.Info) { LogTime = _cleanDate }; 
            _logger.Log(_log);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                File.Delete(ConfigurationManager.AppSettings["LogFilePath"]);
            }
            catch (Exception)
            {
                throw new Exception("Error while deleting test log.");
            }
        }

        [Test]
        public void LogTest()
        {
            LogEntry testLog = _logger.GetAllLogs().Last() as LogEntry;
            Assert.True(_log.Equals(testLog));
        }

        [Test]
        public void LastHourLogsTest()
        {
            var lastHourLogs = _logger.GetLogsFromPeriod(new TimeSpan(0, 1, 0, 0));
            Assert.True(_log.Equals(lastHourLogs.First()));
        }

        [Test]
        public void LastHourErrorsTest()
        {
            var testError = new LogEntry("error!", LogType.Error, new Exception("test error")) {LogTime = _cleanDate};
            _logger.Log(testError);
            var lastHourErrors = _logger.GetLogsByTypeFromPeriod(LogType.Error, new TimeSpan(0, 1, 0, 0));
            Assert.True(testError.Equals(lastHourErrors.First()));
        }

        [Test]
        public void ErrorsOnlyTest()
        {
            var testError = new LogEntry("error!", LogType.Error, new Exception("test error")) { LogTime = _cleanDate };
            _logger.Log(testError);
            var errors = _logger.GetLogsByType(LogType.Error);
            Assert.True(testError.Equals(errors.First()));
        }
    }
}
