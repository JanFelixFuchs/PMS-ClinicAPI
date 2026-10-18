using Application.Common.Behaviours.LoggingBehaviour;
using Application.Common.Logging;
using Application.UnitTests.Utils.Models.Logging;
using Application.UnitTests.Utils.Models.Requests;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Tests.Common.Behaviours.LoggingBehaviour;

public class LoggingBehaviourTests
{
    /* - - - Method: Handle - - - */
    [Fact]
    public async Task Handle_WithNextThrowingException_LogsStartedAndFailedAndRethrowsException()
    {
        // Arrange
        var logger = new TestLogger<LoggingBehaviour<TestRequest, TestResponse>>();
        var sut = new LoggingBehaviour<TestRequest, TestResponse>(logger);
        
        var expectedException = new Exception();
        Task<TestResponse> Next(CancellationToken _) => Task.FromException<TestResponse>(expectedException);
        
        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        var exception = (await act.Should().ThrowAsync<Exception>()).Which;
        exception.Should().BeSameAs(expectedException);
        
        logger.LogEntries.Should().HaveCount(2);
        logger.LogEntries.Select(logEntry => logEntry.Template).Should().Equal(
            LogMessages.RequestStarted,
            LogMessages.RequestFailed);
        logger.LogEntries.Should().NotContain(logEntry => logEntry.Template == LogMessages.RequestCompleted);
        logger.LogEntries.Should().OnlyContain(logEntry => logEntry.LogLevel == LogLevel.Information);
    }
    
    [Fact]
    public async Task Handle_WithNextSucceeding_LogsStartedAndCompletedAndReturnsResponse()
    {
        // Arrange
        var response = new TestResponse();
        var logger = new TestLogger<LoggingBehaviour<TestRequest, TestResponse>>();
        var sut = new LoggingBehaviour<TestRequest, TestResponse>(logger);
        
        Task<TestResponse> Next(CancellationToken _) => Task.FromResult(response);
        
        // Act
        var result = await sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        result.Should().Be(response);
        
        logger.LogEntries.Should().HaveCount(2);
        logger.LogEntries.Select(logEntry => logEntry.Template).Should().Equal(
            LogMessages.RequestStarted,
            LogMessages.RequestCompleted);
        logger.LogEntries.Should().NotContain(logEntry => logEntry.Template == LogMessages.RequestFailed);
        logger.LogEntries.Should().OnlyContain(logEntry => logEntry.LogLevel == LogLevel.Information);
    }

    [Fact]
    public async Task Handle_WithNextSucceeding_ForwardsCancellationTokenToNext()
    {
        // Arrange
        var logger = new TestLogger<LoggingBehaviour<TestRequest, TestResponse>>();
        var sut = new LoggingBehaviour<TestRequest, TestResponse>(logger);
        
        using var cancellationTokenSource = new CancellationTokenSource();
        var expectedCancellationToken = cancellationTokenSource.Token;
        CancellationToken? receivedCancellationToken = null;
        
        Task<TestResponse> Next(CancellationToken cancellationToken)
        {
            receivedCancellationToken = cancellationToken;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        await sut.Handle(new TestRequest(), Next, expectedCancellationToken);
        
        // Assert
        receivedCancellationToken.Should().Be(expectedCancellationToken);
    }
}