using System.Net;
using Utils.Exceptions.Base;

namespace Application.Common.Exceptions;

public class InvalidPropertyValueException(string typeName, string propertyName)
    : CustomExceptionBase($"{typeName} with invalid {propertyName} provided", HttpStatusCode.BadRequest);