using System.Net;
using Utils.Exceptions.Base;

namespace Domain.Commons.Exceptions;

public class InvalidOperationException(string message)
    : CustomExceptionBase($"Invalid operation: {message}", HttpStatusCode.Conflict);