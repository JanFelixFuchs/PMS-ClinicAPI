using System.Net;
using Application.Common.OutputModels.RoomOutputModels;
using Application.UseCases.RoomUseCases.Commands.ArchiveRoomCommand;
using Application.UseCases.RoomUseCases.Commands.DeleteRoomCommand;
using Application.UseCases.RoomUseCases.Commands.UnarchiveRoomCommand;
using Application.UseCases.RoomUseCases.Queries.ReadRoomQuery;
using Application.UseCases.RoomUseCases.Queries.ReadRoomsQuery;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.RoomInputModels;
using PMS_ClinicAPI.Common.Mappings.RoomMappings;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.RoomControllers;

[ApiController]
[Route("api/rooms")]
public class RoomController(
    ILogger<RoomController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<RoomController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateRoom)]
    [SwaggerOperation("Creates a room")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<RoomDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomDetailedOutputModel>>> CreateRoom([FromBody] CreateRoomInputModel createRoomInputModel)
    {
        return await Execute(
            createRoomInputModel.ToCreateRoomCommand(), 
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadRoom)]
    [SwaggerOperation("Reads all rooms")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<RoomOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<RoomOverviewOutputModel>>>> ReadRooms([FromQuery] bool archived = false)
    {
        return await Execute(
            new ReadRoomsQuery(archived), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadRoom)]
    [SwaggerOperation("Reads a room by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<RoomDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomDetailedOutputModel>>> ReadRoom([FromRoute] Guid id)
    {
        return await Execute(
            new ReadRoomQuery(id), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateRoom)]
    [SwaggerOperation("Updates a room by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<RoomDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomDetailedOutputModel>>> UpdateRoom(
        [FromRoute] Guid id,
        [FromBody] UpdateRoomInputModel updateRoomInputModel)
    {
        return await Execute(
            updateRoomInputModel.ToUpdateRoomCommand(id), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/archive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveRoom)]
    [SwaggerOperation("Archives a room by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Archiving succeeded", typeof(HttpResult<RoomDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomDetailedOutputModel>>> ArchiveRoom([FromRoute] Guid id)
    {
        return await Execute(
            new ArchiveRoomCommand(id), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/unarchive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveRoom)]
    [SwaggerOperation("Unarchives a room by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Unarchiving succeeded", typeof(HttpResult<RoomDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomDetailedOutputModel>>> UnarchiveRoom([FromRoute] Guid id)
    {
        return await Execute(
            new UnarchiveRoomCommand(id), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteRoom)]
    [SwaggerOperation("Deletes a room by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteRoom([FromRoute] Guid id)
    {
        return await Execute(new DeleteRoomCommand(id), HttpStatusCode.OK);
    }
}