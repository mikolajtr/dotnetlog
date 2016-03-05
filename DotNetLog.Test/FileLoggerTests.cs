using System;
using System.Configuration;
using System.IO;
using System.Linq;
using DotNetLog.LogEntries;
using DotNetLog.Loggers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DotNetLog.Test
{
    [TestFixture]
    public class FileLoggerTests
    {

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["LogFilePath"] = "log_test.txt";
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
            FileLogger logger = FileLogger.Instance;
            LogEntry log = new LogEntry("test log", LogType.Info);
            log.LogTime = new DateTime(); // mocking datetime to avoid trouble with miliseconds
            logger.Log(log);
            LogEntry testLog = logger.GetAllLogs().Last() as LogEntry;
            Assert.True(log.Equals(testLog));
        }
    }
}
