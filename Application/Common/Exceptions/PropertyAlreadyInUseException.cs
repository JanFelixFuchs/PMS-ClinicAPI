using System.Net;
using Utils.Exceptions.Base;

namespace Application.Common.Exceptions;

public class PropertyAlreadyInUseException<T> : CustomExceptionBase
{ 
    public PropertyAlreadyInUseException(string typeName, string propertyName, T propertyValue)
        : base($"{typeName} with {propertyName} {propertyValue?.ToString()} is already in use", HttpStatusCode.Conflict) { }
    
    public PropertyAlreadyInUseException(string typeName, string propertyName, ICollection<T> propertyValues)
        : base($"{typeName} with {propertyName} [{string.Join(", ", propertyValues.Select(propertyValue => propertyValue?.ToString()))}] is already in use", HttpStatusCode.Conflict) { }
}