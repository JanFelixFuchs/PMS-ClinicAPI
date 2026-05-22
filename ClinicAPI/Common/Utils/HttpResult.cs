using System.Net;
using Utils.Exceptions.Base;

namespace PMS_ClinicAPI.Common.Utils;

public class HttpResult<T>
{
    public int HttpStatusCode { get; set; }
    public string? Exception { get; set; }
    public string Message { get; set; }
    public ICollection<string>? Errors { get; set; }
    public T? Payload { get; set; }

    // Standard constructor used to initialize objects with a payload
    public HttpResult(HttpStatusCode httpStatusCode, T? payload = default)
    {
        // Assigning status code, message and payload
        HttpStatusCode = (int) httpStatusCode;
        Message = "Request proceeded successfully";
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
            Exception = customExceptionBase.GetType().Name;
            Message = customExceptionBase.Message;
            
            // Checking if custom exception contains errors
            if (customExceptionBase.Errors != null && customExceptionBase.Errors.Count != 0)
                Errors = customExceptionBase.Errors;
            
            // Returning
            return;
        }
        
        // Assigning properties from exception
        HttpStatusCode = (int) System.Net.HttpStatusCode.InternalServerError;
        Exception = exception.GetType().Name;
        Message = exception.Message;
    }
}