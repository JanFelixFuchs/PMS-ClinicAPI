using System.Net;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.UseCases.AppointmentProtocolUseCases.Commands.CompleteAppointmentProtocolCommand;
using Application.UseCases.AppointmentProtocolUseCases.Commands.StartAppointmentProtocolCommand;
using Application.UseCases.AppointmentProtocolUseCases.Queries.ReadAppointmentProtocolQuery;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;
using PMS_ClinicAPI.Common.Mappings.AppointmentMappings;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.AppointmentControllers;

[ApiController]
[Route("api/appointmentProtocols")]
public class AppointmentProtocolController(
    ILogger<AppointmentProtocolController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<AppointmentProtocolController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadAppointmentProtocol)]
    [SwaggerOperation("Reads an appointment protocol by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<AppointmentProtocolDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentProtocolDetailedOutputModel>>> ReadAppointmentProtocol([FromRoute] Guid id)
    {
        return await Execute(
            new ReadAppointmentProtocolQuery(id), 
            HttpStatusCode.OK, 
            nameof(ReadAppointmentProtocol),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateAppointmentProtocol)]
    [SwaggerOperation("Updates an appointment protocol by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<AppointmentProtocolDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentProtocolDetailedOutputModel>>> UpdateAppointmentProtocol(
        [FromRoute] Guid id,
        [FromBody] UpdateAppointmentProtocolInputModel updateAppointmentProtocolInputModel)
    {
        return await Execute(
            updateAppointmentProtocolInputModel.ToUpdateAppointmentProtocolCommand(id), 
            HttpStatusCode.OK, 
            nameof(UpdateAppointmentProtocol),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/start")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateAppointmentProtocol)]
    [SwaggerOperation("Starts an appointment protocol by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Starting succeeded", typeof(HttpResult<AppointmentProtocolDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentProtocolDetailedOutputModel>>> StartAppointmentProtocol([FromRoute] Guid id)
    {
        return await Execute(
            new StartAppointmentProtocolCommand(id), 
            HttpStatusCode.OK, 
            nameof(StartAppointmentProtocol),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/complete")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateAppointmentProtocol)]
    [SwaggerOperation("Completes an appointment protocol by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Completing succeeded", typeof(HttpResult<AppointmentProtocolDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentProtocolDetailedOutputModel>>> CompleteAppointmentProtocol([FromRoute] Guid id)
    {
        return await Execute(
            new CompleteAppointmentProtocolCommand(id), 
            HttpStatusCode.OK, 
            nameof(CompleteAppointmentProtocol),
            payloadSelector: result => result);
    }
}