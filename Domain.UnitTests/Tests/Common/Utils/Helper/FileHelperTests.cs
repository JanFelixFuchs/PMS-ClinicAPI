using Domain.Common.Enums;
using Domain.Common.Utils.Helper;
using FluentAssertions;
using TestUtils.Constants;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.Utils.Helper;

public class FileHelperTests
{
    /* - - - Preparation - - - */
    private const string PropertyName = "test-file";
    
    
    /* - - - Method: InferFileContentType - - - */
    [Fact]
    public void InferFileContentType_WithUndefinedFileType_ThrowsValidationException()
    {
        // Arrange
        byte[] undefinedBytes = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05];

        // Act
        var act = () => FileHelper.InferFileContentType(undefinedBytes, PropertyName);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = PropertyName,
            ErrorCode = ErrorCode.UNSUPPORTED_FILE_TYPE
        }); 
    }

    [Fact]
    public void InferFileContentType_WithEmptyFile_ThrowsValidationException()
    {
        // Act
        var act = () => FileHelper.InferFileContentType([], PropertyName);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = PropertyName,
            ErrorCode = ErrorCode.UNSUPPORTED_FILE_TYPE
        }); 
    }

    [Theory]
    [MemberData(nameof(TestConstants.ValidAppendixContentTypes), MemberType = typeof(TestConstants))]
    public void InferFileContentType_WithMinimalHeaderBytes_ReturnsInferredFileType(AppendixContentType appendixContentType, byte[] bytes)
    {
        // Act
        var result = FileHelper.InferFileContentType(bytes, PropertyName);
        
        // Assert
        result.Should().Be(appendixContentType);
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.ValidAppendixContentTypes), MemberType = typeof(TestConstants))]
    public void InferFileContentType_WithMinimalHeaderBytesFollowedByAdditionalBytes_ReturnsInferredFileType(AppendixContentType appendixContentType, byte[] bytes)
    {
        // Arrange
        byte[] additionalBytes = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05]; 
        
        // Act
        var result = FileHelper.InferFileContentType(bytes.Concat(additionalBytes).ToArray(), PropertyName);
        
        // Assert
        result.Should().Be(appendixContentType);
    }
}