using System.Net;
using Utils.Exceptions.Base;

namespace Infrastructure.Common.Exceptions.Database;

public class DatabaseCreateException(string typeName, string message)
    : CustomExceptionBase($"Creating {typeName} failed: {message}", HttpStatusCode.InternalServerError);