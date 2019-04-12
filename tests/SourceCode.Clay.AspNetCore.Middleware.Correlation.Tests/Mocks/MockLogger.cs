using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockLogger : ILogger<CorrelationMiddleware>
    {
        private object _lock = new object();
        public List<MockLoggerMessage> Messages { get; } = new List<MockLoggerMessage>();

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
                Messages.Add(new MockLoggerMessage(logLevel, eventId, state, exception, (object s, Exception e) => { return formatter((TState)s, e); }));
            }
        }
    }
}
