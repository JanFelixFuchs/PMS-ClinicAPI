using System.Net;
using Utils.Exceptions.Base;

namespace Application.Common.Exceptions;

public class PropertyUnchangedException(string typeName, string propertyName)
    : CustomExceptionBase($"{propertyName} of {typeName} must be different from current value", HttpStatusCode.BadRequest);