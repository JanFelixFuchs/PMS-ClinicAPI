using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Codes;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Application.Common.Exceptions;

public class PropertyValueAlreadyInUseException<T> : CustomExceptionBase
{ 
    public PropertyValueAlreadyInUseException(string typeName, string propertyName, T propertyValue)
        : base(
            $"{typeName} with {propertyName} {propertyValue?.ToString()} is already in use", 
            HttpStatusCode.Conflict,
            ErrorType.PROPERTY_VALUE_ALREADY_IN_USE,
            [new FieldError(propertyName, ErrorCode.VALUE_ALREADY_IN_USE)]) { }
    
    public PropertyValueAlreadyInUseException(string typeName, string propertyName, ICollection<T> propertyValues)
        : base(
            $"{typeName} with {propertyName} [{string.Join(", ", propertyValues.Select(propertyValue => propertyValue?.ToString()))}] is already in use", 
            HttpStatusCode.Conflict,
            ErrorType.PROPERTY_VALUE_ALREADY_IN_USE,
            [new FieldError(propertyName, ErrorCode.VALUE_ALREADY_IN_USE)]) { }
}