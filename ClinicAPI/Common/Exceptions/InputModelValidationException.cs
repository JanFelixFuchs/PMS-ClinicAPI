using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Field;
using Utils.Exceptions.Errors.Types;

namespace PMS_ClinicAPI.Common.Exceptions;

public class InputModelValidationException(ICollection<FieldError> errors)
    : CustomExceptionBase(
        "Input model validation failed",
        HttpStatusCode.BadRequest,
        ErrorType.VALIDATION_ERROR,
        errors);