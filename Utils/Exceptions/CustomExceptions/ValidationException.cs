using System.Net;
using Utils.Exceptions.Base;

namespace Utils.Exceptions.CustomExceptions;

public class ValidationException(ICollection<string> errors)
    : CustomExceptionBase("Validation failed", HttpStatusCode.UnprocessableEntity, errors);