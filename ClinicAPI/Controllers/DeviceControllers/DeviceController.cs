using System.Net;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.UseCases.DeviceUseCases.Commands.ArchiveDeviceCommand;
using Application.UseCases.DeviceUseCases.Commands.DeleteDeviceCommand;
using Application.UseCases.DeviceUseCases.Commands.UnarchiveDeviceCommand;
using Application.UseCases.DeviceUseCases.Commands.UpdateDeviceStatusCommand;
using Application.UseCases.DeviceUseCases.Queries.ReadDeviceQuery;
using Application.UseCases.DeviceUseCases.Queries.ReadDevicesQuery;
using Domain.Commons.Enums;
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
[Route("api/devices")]
public class DeviceController(
    ILogger<DeviceController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<DeviceController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateDevice)]
    [SwaggerOperation("Creates a device")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<DeviceDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceDetailedOutputModel>>> CreateDevice([FromBody] CreateDeviceInputModel createDeviceInputModel)
    {
        return await Execute(
            createDeviceInputModel.ToCreateDeviceCommand(),
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadDevice)]
    [SwaggerOperation("Reads all devices")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<DeviceOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<DeviceOverviewOutputModel>>>> ReadDevices([FromQuery] bool archived = false)
    {
        return await Execute(
            new ReadDevicesQuery(archived),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadDevice)]
    [SwaggerOperation("Reads a device by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<DeviceDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceDetailedOutputModel>>> ReadDevice([FromRoute] Guid id)
    {
        return await Execute(
            new ReadDeviceQuery(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateDevice)]
    [SwaggerOperation("Updates a device by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<DeviceDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceDetailedOutputModel>>> UpdateDevice(
        [FromRoute] Guid id,
        [FromBody] UpdateDeviceInputModel updateDeviceInputModel)
    {
        return await Execute(
            updateDeviceInputModel.ToUpdateDeviceCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/status/{status}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateDevice)]
    [SwaggerOperation("Updates a device status by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<DeviceDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceDetailedOutputModel>>> UpdateDeviceStatus(
        [FromRoute] Guid id,
        [FromRoute] DeviceStatus status)
    {
        return await Execute(
            new UpdateDeviceStatusCommand(id,
                status),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/archive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveDevice)]
    [SwaggerOperation("Archives a device by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Archiving succeeded", typeof(HttpResult<DeviceDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceDetailedOutputModel>>> ArchiveDevice([FromRoute] Guid id)
    {
        return await Execute(
            new ArchiveDeviceCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/unarchive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveDevice)]
    [SwaggerOperation("Unarchives a device by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Unarchiving succeeded", typeof(HttpResult<DeviceDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<DeviceDetailedOutputModel>>> UnarchiveDevice([FromRoute] Guid id)
    {
        return await Execute(
            new UnarchiveDeviceCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteDevice)]
    [SwaggerOperation("Deletes a device by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteDevice([FromRoute] Guid id)
    {
        return await Execute(new DeleteDeviceCommand(id), HttpStatusCode.OK);
    }
}