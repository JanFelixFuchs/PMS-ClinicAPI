using System.Net;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.UseCases.AppointmentUseCases.Commands.DeleteAppointmentCommand;
using Application.UseCases.AppointmentUseCases.Commands.MarkAppointmentAsAttendedCommand;
using Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentQuery;
using Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentsQuery;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;
using PMS_ClinicAPI.Common.Mappings.AppointmentMappings;
using PMS_ClinicAPI.Common.Utils.Conversions.DateTimes;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.AppointmentControllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController(
    ILogger<AppointmentController> logger,
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<AppointmentController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateAppointment)]
    [SwaggerOperation("Creates an appointment")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<AppointmentDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentDetailedOutputModel>>> CreateAppointment([FromBody] CreateAppointmentInputModel createAppointmentInputModel)
    {
        return await Execute(
            createAppointmentInputModel.ToCreateAppointmentCommand(),
            HttpStatusCode.Created,
            nameof(CreateAppointment),
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadAppointment)]
    [SwaggerOperation("Reads appointments by date range")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<AppointmentOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<AppointmentOverviewOutputModel>>>> ReadAppointmentsByDateRange(
        [FromQuery] DateTime startDateTime,
        [FromQuery] DateTime endDateTime)
    {
        return await Execute(
            new ReadAppointmentsQuery(
                startDateTime.EnsureUtc(), 
                endDateTime.EnsureUtc()),
            HttpStatusCode.OK,
            nameof(ReadAppointmentsByDateRange),
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadAppointment)]
    [SwaggerOperation("Reads an appointment by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<AppointmentDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentDetailedOutputModel>>> ReadAppointment([FromRoute] Guid id)
    {
        return await Execute(
            new ReadAppointmentQuery(id),
            HttpStatusCode.OK,
            nameof(ReadAppointment),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateAppointment)]
    [SwaggerOperation("Updates an appointment by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<AppointmentDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentDetailedOutputModel>>> UpdateAppointment(
        [FromRoute] Guid id,
        [FromBody] UpdateAppointmentInputModel updateAppointmentInputModel)
    {
        return await Execute(
            updateAppointmentInputModel.ToUpdateAppointmentCommand(id), 
            HttpStatusCode.OK, 
            nameof(UpdateAppointment),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/mark-as-attended")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateAppointment)]
    [SwaggerOperation("Marks an appointment as attended by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Marking succeeded", typeof(HttpResult<AppointmentDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentDetailedOutputModel>>> MarkAppointmentAsAttended([FromRoute] Guid id)
    {
        return await Execute(
            new MarkAppointmentAsAttendedCommand(id), 
            HttpStatusCode.OK, 
            nameof(MarkAppointmentAsAttended),
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteAppointment)]
    [SwaggerOperation("Deletes an appointment by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteAppointment([FromRoute] Guid id)
    {
        return await Execute(
            new DeleteAppointmentCommand(id), 
            HttpStatusCode.OK,
            nameof(DeleteAppointment));
    }
}