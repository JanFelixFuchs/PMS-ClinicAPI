using Application.Common.Behaviours.ValidationBehaviour;
using Application.UnitTests.Utils.Models.Requests;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Utils.Exceptions.Errors.Codes;
using ValidationException = Utils.Exceptions.CustomExceptions.ValidationException;

namespace Application.UnitTests.Tests.Common.Behaviours.Validation;

public class ValidationBehaviourTests
{
    /* - - - Preparation - - - */
    private const string PropertyNameOne = "test-property-one";
    private const string PropertyNameTwo = "test-property-two";
    
    private const string ErrorMessageOne = "test-error-message-one";
    private const string ErrorMessageTwo = "test-error-message-two";
    
    private const ErrorCode ErrorCodeOne = ErrorCode.MISSING_VALUE;
    private const ErrorCode ErrorCodeTwo = ErrorCode.EMPTY_VALUE;
    
    
    /* - - - Method: Handle - - - */
    [Fact]
    public async Task Handle_WithSingleValidatorReturningSingleError_ThrowsValidationExceptionAndDoesNotCallNext()
    {
        // Arrange
        var validationFailure = new ValidationFailure(PropertyNameOne, ErrorMessageOne)
        {
            CustomState = ErrorCodeOne
        };
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailure]));
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validator]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _) 
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeFalse();
        var exception = (await act.Should().ThrowAsync<ValidationException>()).Which;
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameOne &&
            fieldError.ErrorCode == ErrorCodeOne
        );
        exception.Message.Should().Be(ErrorMessageOne);
    }
    
    [Fact]
    public async Task Handle_WithSingleValidatorReturningMultipleErrors_ThrowsValidationExceptionAndDoesNotCallNext()
    {
        // Arrange
        var validationFailureOne = new ValidationFailure(PropertyNameOne, ErrorMessageOne)
        {
            CustomState = ErrorCodeOne
        };
        var validationFailureTwo = new ValidationFailure(PropertyNameTwo, ErrorMessageTwo)
        {
            CustomState = ErrorCodeTwo
        };
        
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailureOne, validationFailureTwo]));
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validator]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _) 
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeFalse();
        var exception = (await act.Should().ThrowAsync<ValidationException>()).Which;
        
        exception.FieldErrors.Should().HaveCount(2);
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameOne &&
            fieldError.ErrorCode == ErrorCodeOne
        );
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameTwo &&
            fieldError.ErrorCode == ErrorCodeTwo
        );
        
        exception.Message.Should().Contain(ErrorMessageOne);
        exception.Message.Should().Contain(ErrorMessageTwo);
    }
    
    [Fact]
    public async Task Handle_WithMultipleValidatorsEachReturningSingleError_ThrowsValidationExceptionAndDoesNotCallNext()
    {
        // Arrange
        var validationFailureOne = new ValidationFailure(PropertyNameOne, ErrorMessageOne)
        {
            CustomState = ErrorCodeOne
        };
        var validationFailureTwo = new ValidationFailure(PropertyNameTwo, ErrorMessageTwo)
        {
            CustomState = ErrorCodeTwo
        };
        
        var validatorOne = Substitute.For<IValidator<TestRequest>>();
        validatorOne
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailureOne]));
        
        var validatorTwo = Substitute.For<IValidator<TestRequest>>();
        validatorTwo
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailureTwo]));
        
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validatorOne, validatorTwo]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _) 
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeFalse();
        var exception = (await act.Should().ThrowAsync<ValidationException>()).Which;
        
        exception.FieldErrors.Should().HaveCount(2);
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameOne &&
            fieldError.ErrorCode == ErrorCodeOne
        );
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameTwo &&
            fieldError.ErrorCode == ErrorCodeTwo
        );
        
        exception.Message.Should().Contain(ErrorMessageOne);
        exception.Message.Should().Contain(ErrorMessageTwo);
    }
    
    [Fact]
    public async Task Handle_WithOneValidValidatorAndOneInvalidValidator_ThrowsValidationExceptionAndDoesNotCallNext()
    {
        // Arrange
        var validationFailure = new ValidationFailure(PropertyNameOne, ErrorMessageOne)
        {
            CustomState = ErrorCodeOne
        };

        var validValidator = Substitute.For<IValidator<TestRequest>>();
        validValidator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult());
        
        var invalidValidator = Substitute.For<IValidator<TestRequest>>();
        invalidValidator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailure])); 
        
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validValidator, invalidValidator]);

        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }

        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);

        // Assert
        nextCalled.Should().BeFalse();
        var exception = (await act.Should().ThrowAsync<ValidationException>()).Which;
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameOne &&
            fieldError.ErrorCode == ErrorCodeOne
        );
        exception.Message.Should().Contain(ErrorMessageOne);
    }
    
    [Fact]
    public async Task Handle_WithValidationFailureHavingNonErrorCodeCustomState_ThrowsValidationExceptionWithUnknownErrorAndDoesNotCallNext()
    {
        // Arrange
        var validationFailure = new ValidationFailure(PropertyNameOne, ErrorMessageOne)
        {
            CustomState = "invalid-error-code"
        };
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailure]));
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validator]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _) 
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
    
        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeFalse();
        var exception = (await act.Should().ThrowAsync<ValidationException>()).Which;
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameOne &&
            fieldError.ErrorCode == ErrorCode.UNKNOWN_ERROR
        );
        exception.Message.Should().Be(ErrorMessageOne);
    }

    [Fact]
    public async Task Handle_WithValidationFailureHavingNoCustomState_ThrowsValidationExceptionWithUnknownErrorAndDoesMapsToUnknownError()
    {
        // Arrange
        var validationFailure = new ValidationFailure(PropertyNameOne, ErrorMessageOne);
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult([validationFailure]));
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validator]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _) 
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
    
        // Act
        var act = () => sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeFalse();
        var exception = (await act.Should().ThrowAsync<ValidationException>()).Which;
        exception.FieldErrors.Should().ContainSingle(fieldError =>
            fieldError.Field == PropertyNameOne &&
            fieldError.ErrorCode == ErrorCode.UNKNOWN_ERROR
        );
        exception.Message.Should().Be(ErrorMessageOne);
    }
    
    [Fact]
    public async Task Handle_WithNoValidators_CallsNextAndReturnsResponse()
    {
        // Arrange
        var response = new TestResponse();
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true; return Task.FromResult(response);
        }

        // Act
        var result = await sut.Handle(new TestRequest(), Next, CancellationToken.None);

        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be(response);
    }
    
    [Fact]
    public async Task Handle_WithValidatorAndNoValidationErrors_CallsNextAndReturnsResponse()
    {
        // Arrange
        var response = new TestResponse();
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator
            .Validate(Arg.Any<TestRequest>())
            .Returns(new ValidationResult());
        var sut = new ValidationBehaviour<TestRequest, TestResponse>([validator]);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true; return Task.FromResult(response);
        }

        // Act
        var result = await sut.Handle(new TestRequest(), Next, CancellationToken.None);

        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be(response);
    }
}