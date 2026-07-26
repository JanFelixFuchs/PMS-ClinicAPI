using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Codes;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace Application.Common.Exceptions;

public class IncorrectPropertyValueException(string typeName, string propertyName)
    : CustomExceptionBase(
        $"Incorrect value for property {propertyName} of type {typeName}",
        HttpStatusCode.BadRequest,
        ErrorType.INCORRECT_PROPERTY_VALUE,
        [new FieldError(propertyName, ErrorCode.INCORRECT_VALUE)]);
