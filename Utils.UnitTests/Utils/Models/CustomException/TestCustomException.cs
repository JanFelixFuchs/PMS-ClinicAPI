using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Utils.UnitTests.Utils.Models.CustomException;

public class TestCustomException(
    string logMessage,
    HttpStatusCode httpStatusCode,
    ErrorType errorType,
    ICollection<FieldError>? fieldErrors = null,
    Exception? innerException = null)
    : CustomExceptionBase(
        logMessage,
        httpStatusCode,
        errorType,
        fieldErrors,
        innerException: innerException);