using System.Net;
using Application.Common.OutputModels.RoomOutputModels;
using Application.UseCases.RoomCategoryUseCases.Commands.DeleteRoomCategoryCommand;
using Application.UseCases.RoomCategoryUseCases.Queries.ReadRoomCategoriesQuery;
using Application.UseCases.RoomCategoryUseCases.Queries.ReadRoomCategoryQuery;
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
[Route("api/roomCategories")]
public class RoomCategoryController(
    ILogger<RoomCategoryController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender) 
    : CustomControllerBase<RoomCategoryController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateRoomCategory)]
    [SwaggerOperation("Creates a room category")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<RoomCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomCategoryDetailedOutputModel>>> CreateRoomCategory([FromBody] CreateRoomCategoryInputModel createRoomCategoryInputModel)
    {
        return await Execute(
            createRoomCategoryInputModel.ToCreateRoomCategoryCommand(),
            HttpStatusCode.Created,
            nameof(CreateRoomCategory),
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadRoomCategory)]
    [SwaggerOperation("Reads all room categories")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<RoomCategoryOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<RoomCategoryOverviewOutputModel>>>> ReadRoomCategories()
    {
        return await Execute(
            new ReadRoomCategoriesQuery(), 
            HttpStatusCode.OK, 
            nameof(ReadRoomCategories),
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadRoomCategory)]
    [SwaggerOperation("Reads a room category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<RoomCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomCategoryDetailedOutputModel>>> ReadRoomCategory([FromRoute] Guid id)
    {
        return await Execute(
            new ReadRoomCategoryQuery(id), 
            HttpStatusCode.OK, 
            nameof(ReadRoomCategory),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateRoomCategory)]
    [SwaggerOperation("Updates a room category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<RoomCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoomCategoryDetailedOutputModel>>> UpdateRoomCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateRoomCategoryInputModel updateRoomCategoryInputModel)
    {
        return await Execute(
            updateRoomCategoryInputModel.ToUpdateRoomCategoryCommand(id), 
            HttpStatusCode.OK, 
            nameof(UpdateRoomCategory),
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteRoomCategory)]
    [SwaggerOperation("Deletes a room category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteRoomCategory([FromRoute] Guid id)
    {
        return await Execute(
            new DeleteRoomCategoryCommand(id), 
            HttpStatusCode.OK,
            nameof(DeleteRoomCategory));
    }
}