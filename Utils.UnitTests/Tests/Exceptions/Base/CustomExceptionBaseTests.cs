using System.Net;
using FluentAssertions;
using Utils.Exceptions.Errors.Codes;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;
using Utils.UnitTests.Utils.Models.CustomException;

namespace Utils.UnitTests.Tests.Exceptions.Base;

public class CustomExceptionBaseTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        const string logMessage = "test-log-message";
        const HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        const ErrorType errorType = ErrorType.INTERNAL_ERROR;
        var fieldErrors = new List<FieldError> { new("test-property", ErrorCode.UNKNOWN_ERROR) };
        var innerException = new Exception("test-inner-exception");
        
        
        // Act
        var result = new TestCustomException(
            logMessage,
            httpStatusCode,
            errorType,
            fieldErrors,
            innerException);
        
        // Assert
        result.Message.Should().Be(logMessage);
        result.HttpStatusCode.Should().Be(httpStatusCode);
        result.ErrorType.Should().Be(errorType);
        result.FieldErrors.Should().BeEquivalentTo(fieldErrors);
        result.InnerException.Should().Be(innerException);
    }
}