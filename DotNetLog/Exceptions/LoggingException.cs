using System;
using System.Runtime.Serialization;

namespace DotNetLog.Exceptions
{
    public class LoggingException : Exception
    {
        public LoggingException()
        {
        }

        public LoggingException(string message) : base(message)
        {
        }

        public LoggingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoggingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
