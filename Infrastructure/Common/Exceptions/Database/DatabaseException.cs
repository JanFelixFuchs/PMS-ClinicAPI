using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace Infrastructure.Common.Exceptions.Database;

public class DatabaseException(string typeName, Exception innerException)
    : CustomExceptionBase(
        $"Failed to execute a database operation on type {typeName}",
        HttpStatusCode.InternalServerError,
        ErrorType.INTERNAL_ERROR,
        innerException: innerException);