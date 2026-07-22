using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace Infrastructure.Common.Exceptions.Contexts;

public class InvalidRequestContextException(string claimName)
    : CustomExceptionBase(
        $"Failed to construct request context due to missing or invalid {claimName} claim", 
        HttpStatusCode.Unauthorized,
        ErrorType.INTERNAL_ERROR);
