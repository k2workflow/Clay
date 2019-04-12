using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockLogger : ILogger<CorrelationMiddleware>
    {
        private object _lock = new object();
        public List<LogMessage> Messages { get; } = new List<LogMessage>();

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            lock (_lock)
            {
                Messages.Add(new LogMessage(logLevel, eventId, state, exception, (object s, Exception e) => { return formatter((TState)s, e); }));
            }
        }

        public class LogMessage
        {
            public LogMessage(LogLevel logLevel, EventId eventId, object state, Exception exception, Func<object, Exception, string> formatter)
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
}
