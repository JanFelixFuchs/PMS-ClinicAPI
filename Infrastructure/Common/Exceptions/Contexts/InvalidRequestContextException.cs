using System.Net;
using Utils.Exceptions.Base;

namespace Infrastructure.Common.Exceptions.Contexts;

public class InvalidRequestContextException(string claimName)
    : CustomExceptionBase($"Constructing request context failed due to missing or invalid {claimName} claim", HttpStatusCode.Unauthorized);
