using System.Net;
using Utils.Exceptions.Base;

namespace PMS_ClinicAPI.Common.Exceptions;

public class InvalidConfigurationException(string configurationSectionName)
    : CustomExceptionBase($"Invalid {configurationSectionName}-configuration", HttpStatusCode.InternalServerError);