using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace PMS_ClinicAPI.Common.Exceptions;

public class InvalidConfigurationException(string configurationSectionName)
    : CustomExceptionBase(
        $"Invalid or missing configuration {configurationSectionName}", 
        HttpStatusCode.InternalServerError,
        ErrorType.INTERNAL_ERROR);