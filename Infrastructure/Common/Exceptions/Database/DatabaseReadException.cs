using System.Net;
using Utils.Exceptions.Base;

namespace Infrastructure.Common.Exceptions.Database;

public class DatabaseReadException(string typeName, string message)
    : CustomExceptionBase($"Reading {typeName} failed: {message}", HttpStatusCode.InternalServerError);