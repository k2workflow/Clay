using System;
using Microsoft.Extensions.Logging;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockLoggerMessage
    {
        public MockLoggerMessage(LogLevel logLevel, EventId eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            Formatter = formatter;
        }

        public LogLevel LogLevel { get; set; }
        public EventId EventId { get; set; }
        public object State { get; set; }
        public Exception Exception { get; set; }
        public Func<object, Exception, string> Formatter { get; set; }
    }
}
