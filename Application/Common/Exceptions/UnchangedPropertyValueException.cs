using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Codes;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Application.Common.Exceptions;

public class UnchangedPropertyValueException(string typeName, string propertyName)
    : CustomExceptionBase(
        $"Unchanged value for property {propertyName} of type {typeName}",
        HttpStatusCode.BadRequest,
        ErrorType.PROPERTY_VALUE_UNCHANGED,
        [new FieldError(propertyName, ErrorCode.UNCHANGED_VALUE)]);