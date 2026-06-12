using System.Net;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.UseCases.UserUseCases.Commands.ArchiveUserCommand;
using Application.UseCases.UserUseCases.Commands.DeleteUserCommand;
using Application.UseCases.UserUseCases.Commands.UnarchiveUserCommand;
using Application.UseCases.UserUseCases.Queries.ReadUserQuery;
using Application.UseCases.UserUseCases.Queries.ReadUsersQuery;
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
[Route("api/users")]
public class UserController(
    ILogger<UserController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender) 
    : CustomControllerBase<UserController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateUser)]
    [SwaggerOperation("Creates a user")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<UserDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<UserDetailedOutputModel>>> CreateUser([FromBody] CreateUserInputModel createUserInputModel)
    {
        return await Execute(
            createUserInputModel.ToCreateUserCommand(),
            HttpStatusCode.Created,
            nameof(CreateUser),
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadUser)]
    [SwaggerOperation("Reads all users")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<UserOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<UserOverviewOutputModel>>>> ReadUsers([FromQuery] bool archived = false)
    {
        return await Execute(
            new ReadUsersQuery(archived),
            HttpStatusCode.OK,
            nameof(ReadUsers),
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadUser)]
    [SwaggerOperation("Reads a user by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<UserDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<UserDetailedOutputModel>>> ReadUser([FromRoute] Guid id)
    {
        return await Execute(
            new ReadUserQuery(id),
            HttpStatusCode.OK,
            nameof(ReadUser),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateUser)]
    [SwaggerOperation("Updates a user by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<UserDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<UserDetailedOutputModel>>> UpdateUser(
        [FromRoute] Guid id,
        [FromBody] UpdateUserInputModel updateUserInputModel)
    {
        return await Execute(
            updateUserInputModel.ToUpdateUserCommand(id),
            HttpStatusCode.OK,
            nameof(UpdateUser),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/archive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveUser)]
    [SwaggerOperation("Archives a user by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Archiving succeeded", typeof(HttpResult<UserDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<UserDetailedOutputModel>>> ArchiveUser([FromRoute] Guid id)
    {
        return await Execute(
            new ArchiveUserCommand(id),
            HttpStatusCode.OK,
            nameof(ArchiveUser),
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/unarchive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveUser)]
    [SwaggerOperation("Unarchives a user by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Unarchiving succeeded", typeof(HttpResult<UserDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<UserDetailedOutputModel>>> UnarchiveUser([FromRoute] Guid id)
    {
        return await Execute(
            new UnarchiveUserCommand(id),
            HttpStatusCode.OK,
            nameof(UnarchiveUser),
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteUser)]
    [SwaggerOperation("Deletes a user by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteUser([FromRoute] Guid id)
    {
        return await Execute(
            new DeleteUserCommand(id),
            HttpStatusCode.OK,
            nameof(DeleteUser));
    }
}