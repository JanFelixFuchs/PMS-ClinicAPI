using System.Net;
using Utils.Exceptions.Base;

namespace Application.Common.Exceptions;

public class DuplicateEntityException(string typeName, string propertyName, string propertyValue)
    : CustomExceptionBase($"{typeName} with {propertyName} {propertyValue} already exists", HttpStatusCode.Conflict);