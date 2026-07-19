using System.Net;
using Utils.Exceptions.Base;

namespace Utils.Exceptions.CustomExceptions;

public class AuthorizationFailedException()
    : CustomExceptionBase("Authorization failed", HttpStatusCode.Unauthorized);