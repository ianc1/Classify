namespace Classify.tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    public class TestLogger<T> : ILogger<T>
    {
        public TestLogger(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public List<string> Messages { get; } = new List<string>();

        private ITestOutputHelper OutputHelper { get; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            Messages.Add(message);
            OutputHelper.WriteLine(message);
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
    }
}