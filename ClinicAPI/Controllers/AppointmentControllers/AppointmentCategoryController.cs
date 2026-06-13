using System.Net;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.UseCases.AppointmentCategoryUseCases.Commands.DeleteAppointmentCategoryCommand;
using Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoriesQuery;
using Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoryQuery;
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
[Route("api/appointmentCategories")]
public class AppointmentCategoryController(
    ILogger<AppointmentCategoryController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender) 
    : CustomControllerBase<AppointmentCategoryController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateAppointmentCategory)]
    [SwaggerOperation("Creates an appointment category")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<AppointmentCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentCategoryDetailedOutputModel>>> CreateAppointmentCategory([FromBody] CreateAppointmentCategoryInputModel createAppointmentCategoryInputModel)
    {
        return await Execute(
            createAppointmentCategoryInputModel.ToCreateAppointmentCategoryCommand(), 
            HttpStatusCode.Created, 
            nameof(CreateAppointmentCategory),
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadAppointmentCategory)]
    [SwaggerOperation("Reads all appointment categories")]
    [SwaggerResponse((int) HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<AppointmentCategoryOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<AppointmentCategoryOverviewOutputModel>>>> ReadAppointmentCategories()
    {
        return await Execute(new 
            ReadAppointmentCategoriesQuery(), 
            HttpStatusCode.OK, 
            nameof(ReadAppointmentCategories),
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadAppointmentCategory)]
    [SwaggerOperation("Reads an appointment category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<AppointmentCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentCategoryDetailedOutputModel>>> ReadAppointmentCategory([FromRoute] Guid id)
    {
        return await Execute(
            new ReadAppointmentCategoryQuery(id), 
            HttpStatusCode.OK, 
            nameof(ReadAppointmentCategory),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateAppointmentCategory)]
    [SwaggerOperation("Updates an appointment category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<AppointmentCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<AppointmentCategoryDetailedOutputModel>>> UpdateAppointmentCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateAppointmentCategoryInputModel updateAppointmentCategoryInputModel)
    {
        return await Execute(
            updateAppointmentCategoryInputModel.ToUpdateAppointmentCategoryCommand(id), 
            HttpStatusCode.OK, 
            nameof(UpdateAppointmentCategory),
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteAppointmentCategory)]
    [SwaggerOperation("Deletes an appointment category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteAppointmentCategory([FromRoute] Guid id)
    {
        return await Execute(
            new DeleteAppointmentCategoryCommand(id), 
            HttpStatusCode.OK,
            nameof(DeleteAppointmentCategory));
    }
}