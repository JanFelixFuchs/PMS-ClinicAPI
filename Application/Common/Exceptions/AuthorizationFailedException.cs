using System.Net;
using Utils.Exceptions.Base;

namespace Application.Common.Exceptions;

public class AuthorizationFailedException()
    : CustomExceptionBase("Authorization failed", HttpStatusCode.Unauthorized);