using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Utils.Models.Logging;

public class TestLogger<T> : ILogger<T>
{
    public List<TestLogEntry> LogEntries { get; } = [];
    
    public void Log<TState>(
        LogLevel logLevel, 
        EventId eventId, 
        TState state, 
        Exception? exception, 
        Func<TState, Exception?, string> formatter)
    {
        var template = state is IEnumerable<KeyValuePair<string, object?>> values
            ? values.FirstOrDefault(value => value.Key == "{OriginalFormat}").Value?.ToString() ?? string.Empty
            : state?.ToString() ?? string.Empty;
        
        LogEntries.Add(new TestLogEntry(logLevel, template));
    }

    public bool IsEnabled(LogLevel logLevel) => true;
    
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => EmptyDisposable.Instance;
    
    private class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Instance = new();

        public void Dispose() { }
    }
}