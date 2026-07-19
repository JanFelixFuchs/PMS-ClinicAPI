using System.Net;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.UseCases.DeviceCategoryUseCases.Commands.DeleteDeviceCategoryCommand;
using Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoriesQuery;
using Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoryQuery;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.DeviceInputModels;
using PMS_ClinicAPI.Common.Mappings.DeviceMappings;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.DeviceControllers;

[ApiController]
[Route("api/deviceCategories")]
public class DeviceCategoryController(
    ILogger<DeviceCategoryController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender) 
    : CustomControllerBase<DeviceCategoryController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateDeviceCategory)]
    [SwaggerOperation("Creates a device category")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<DeviceCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceCategoryDetailedOutputModel>>> CreateDeviceCategory([FromBody] CreateDeviceCategoryInputModel createDeviceCategoryInputModel)
    {
        return await Execute(
            createDeviceCategoryInputModel.ToCreateDeviceCategoryCommand(), 
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadDeviceCategory)]
    [SwaggerOperation("Reads all device categories")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<DeviceCategoryOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<DeviceCategoryOverviewOutputModel>>>> ReadDeviceCategories()
    { 
        return await Execute(
            new ReadDeviceCategoriesQuery(),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadDeviceCategory)]
    [SwaggerOperation("Reads a device category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<DeviceCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceCategoryDetailedOutputModel>>> ReadDeviceCategory([FromRoute] Guid id)
    {
        return await Execute(
            new ReadDeviceCategoryQuery(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateDeviceCategory)]
    [SwaggerOperation("Updates a device category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<DeviceCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceCategoryDetailedOutputModel>>> UpdateDeviceCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateDeviceCategoryInputModel updateDeviceCategoryInputModel)
    {
        return await Execute(
            updateDeviceCategoryInputModel.ToUpdateDeviceCategoryCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteDeviceCategory)]
    [SwaggerOperation("Deletes a device category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteDeviceCategory([FromRoute] Guid id)
    {
        return await Execute(new DeleteDeviceCategoryCommand(id), HttpStatusCode.OK);
    }
}