using System.Net;
using Utils.Exceptions.Base;

namespace PMS_ClinicAPI.Common.Exceptions;

public class InputModelValidationException(ICollection<string> errors)
    : CustomExceptionBase("Input model validation failed", HttpStatusCode.BadRequest, errors);