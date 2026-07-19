using System.Net;
using Application.Common.OutputModels.IdentityOutputModels;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.IdentityInputModels;
using PMS_ClinicAPI.Common.Mappings.IdentityMappings;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.IdentityControllers;

[ApiController]
[Route("api/clinics")]
public class ClinicController(
    ILogger<ClinicController> logger,
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<ClinicController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPut]
    [Authorize(Policy = PolicyDefinitions.CanUpdateClinic)]
    [SwaggerOperation("Updates the current clinic")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<ClinicOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicOutputModel>>> UpdateClinic([FromBody] UpdateClinicInputModel updateClinicInputModel)
    {
        return await Execute(
            updateClinicInputModel.ToUpdateClinicCommand(), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
}