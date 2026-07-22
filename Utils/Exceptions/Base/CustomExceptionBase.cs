using System.Net;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Utils.Exceptions.Base;

public abstract class CustomExceptionBase(
    string logMessage,
    HttpStatusCode httpStatusCode,
    ErrorType errorType,
    ICollection<FieldError>? fieldErrors = null,
    Exception? innerException = null) 
    : Exception(logMessage, innerException)
{
    public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
    public ErrorType ErrorType { get; set; } = errorType;
    public ICollection<FieldError>? FieldErrors { get; set; } = fieldErrors;
}