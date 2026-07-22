using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace PMS_ClinicAPI.Common.Utils.Returns;

public class HttpResult<T>
{
    public int HttpStatusCode { get; set; }
    public ErrorType ErrorType { get; set; }
    public ICollection<FieldError>? FieldErrors { get; set; }
    public T? Payload { get; set; }

    // Standard constructor used to initialize objects with a payload
    public HttpResult(HttpStatusCode httpStatusCode, T? payload = default)
    {
        // Assigning status code, error code and payload
        HttpStatusCode = (int) httpStatusCode;
        ErrorType = ErrorType.SUCCESS;
        Payload = payload;
    }

    // Standard constructor used to initialize objects with an exception
    public HttpResult(Exception exception)
    {
        // Checking if exception is a custom exception
        if (exception is CustomExceptionBase customExceptionBase)
        {
            // Assigning properties from custom exception
            HttpStatusCode = (int) customExceptionBase.HttpStatusCode;
            ErrorType = customExceptionBase.ErrorType;
            
            // Checking if custom exception contains field errors
            if (customExceptionBase.FieldErrors != null && customExceptionBase.FieldErrors.Count != 0)
                FieldErrors = customExceptionBase.FieldErrors;
            
            // Returning
            return;
        }
        
        // Assigning properties from exception
        HttpStatusCode = (int) System.Net.HttpStatusCode.InternalServerError;
        ErrorType = ErrorType.INTERNAL_ERROR;
    }
}