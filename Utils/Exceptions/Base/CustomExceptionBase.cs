using System.Net;

namespace Utils.Exceptions.Base;

public abstract class CustomExceptionBase(
    string message,
    HttpStatusCode httpStatusCode,
    ICollection<string>? errors = null) : Exception(message)
{
    public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
    public ICollection<string>? Errors { get; set; } = errors;
}