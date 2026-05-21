using System.Net;
using Utils.Exceptions.Base;

namespace Infrastructure.Common.Exceptions.Database;

public class DatabaseDeleteException(string typeName, string message)
    : CustomExceptionBase($"Deleting {typeName} failed: {message}", HttpStatusCode.InternalServerError);