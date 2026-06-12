using System.Net;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.UseCases.RoleUseCases.Commands.DeleteRoleCommand;
using Application.UseCases.RoleUseCases.Queries.ReadRoleQuery;
using Application.UseCases.RoleUseCases.Queries.ReadRolesQuery;
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
[Route("api/roles")]
public class RoleController(
    ILogger<RoleController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<RoleController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateRole)]
    [SwaggerOperation("Creates a role")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<RoleDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoleDetailedOutputModel>>> CreateRole([FromBody] CreateRoleInputModel createRoleInputModel)
    {
        return await Execute(
            createRoleInputModel.ToCreateRoleCommand(), 
            HttpStatusCode.Created, 
            nameof(CreateRole),
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadRole)]
    [SwaggerOperation("Reads all roles")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<RoleOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<RoleOverviewOutputModel>>>> ReadRoles()
    {
        return await Execute(
            new ReadRolesQuery(),
            HttpStatusCode.OK,
            nameof(ReadRoles),
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadRole)]
    [SwaggerOperation("Reads a role by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<RoleDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoleDetailedOutputModel>>> ReadRole([FromRoute] Guid id)
    {
        return await Execute(
            new ReadRoleQuery(id),
            HttpStatusCode.OK,
            nameof(ReadRole),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateRole)]
    [SwaggerOperation("Updates a role by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<RoleDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<RoleDetailedOutputModel>>> UpdateRole(
        [FromRoute] Guid id,
        [FromBody] UpdateRoleInputModel updateRoleInputModel)
    {
        return await Execute(
            updateRoleInputModel.ToUpdateRoleCommand(id),
            HttpStatusCode.OK,
            nameof(UpdateRole),
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteRole)]
    [SwaggerOperation("Deletes a role by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteRole([FromRoute] Guid id)
    {
        return await Execute(
            new DeleteRoleCommand(id),
            HttpStatusCode.OK,
            nameof(DeleteRole));
    }
}