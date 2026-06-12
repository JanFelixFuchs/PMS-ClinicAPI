using System.Net;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.UseCases.AuthUseCases.Commands.LogoutUserCommand;
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
[Route("api/auth")]
public class AuthController(
    ILogger<AuthController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender) 
    : CustomControllerBase<AuthController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation("Registers a new clinic")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Registration succeeded", typeof(HttpResult<RegisterClinicOutputModel>))]
    public async Task<ActionResult<HttpResult<RegisterClinicOutputModel>>> RegisterClinic([FromBody] RegisterClinicInputModel registerClinicInputModel)
    {
        return await Execute(
            registerClinicInputModel.ToRegisterClinicCommand(), 
            HttpStatusCode.Created, 
            nameof(RegisterClinic),
            payloadSelector: result => result.Payload,
            postProcessingAction: result => SetRefreshTokenCookie(result.RefreshToken));
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation("Logs in a user")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Login succeeded", typeof(HttpResult<LoginUserOutputModel>))]
    public async Task<ActionResult<HttpResult<LoginUserOutputModel>>> LoginUser([FromBody] LoginUserInputModel loginUserInputModel)
    {
        return await Execute(
            loginUserInputModel.ToLoginUserCommand(), 
            HttpStatusCode.OK, 
            nameof(LoginUser),
            payloadSelector: result => result.Payload,
            postProcessingAction: result => SetRefreshTokenCookie(result.RefreshToken));
    }
    
    [HttpPost("refresh")]
    [AllowAnonymous]
    [SwaggerOperation("Refreshes a user's access and refresh tokens")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Refresh succeeded", typeof(HttpResult<RefreshTokensOutputModel>))]
    public async Task<ActionResult<HttpResult<RefreshTokensOutputModel>>> RefreshTokens([FromBody] RefreshTokensInputModel refreshTokensInputModel)
    {
        return await Execute(
            refreshTokensInputModel.ToRefreshTokensCommand(GetRefreshTokenCookie()), 
            HttpStatusCode.OK, 
            nameof(RefreshTokens),
            payloadSelector: result => result.Payload,
            postProcessingAction: result => SetRefreshTokenCookie(result.RefreshToken));
    }
    
    [HttpPut("code")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateClinic)]
    [SwaggerOperation("Updates the current clinic's code")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> UpdateClinicCode([FromBody] UpdateClinicCodeInputModel updateClinicCodeInputModel)
    {
        return await Execute(
            updateClinicCodeInputModel.ToUpdateClinicCodeCommand(),
            HttpStatusCode.OK,
            nameof(UpdateClinicCode));
    }
    
    [HttpPut("username")]
    [Authorize]
    [SwaggerOperation("Updates the current user's username")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<UpdateUsernameOutputModel>))]
    public async Task<ActionResult<HttpResult<UpdateUsernameOutputModel>>> UpdateUsername([FromBody] UpdateUsernameInputModel updateUsernameInputModel)
    {
        return await Execute(
            updateUsernameInputModel.ToUpdateUsernameCommand(),
            HttpStatusCode.OK,
            nameof(UpdateUsername),
            payloadSelector: result => result);
    }
    
    [HttpPut("password")]
    [Authorize]
    [SwaggerOperation("Updates the current users's password")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<UpdatePasswordOutputModel>))]
    public async Task<ActionResult<HttpResult<UpdatePasswordOutputModel>>> UpdatePassword([FromBody] UpdatePasswordInputModel updatePasswordInputModel)
    {
        return await Execute(
            updatePasswordInputModel.ToUpdatePasswordCommand(), 
            HttpStatusCode.OK, 
            nameof(UpdatePassword),
            payloadSelector: result => result.Payload,
            postProcessingAction: result => SetRefreshTokenCookie(result.RefreshToken));
    }

    [HttpDelete("logout")]
    [Authorize]
    [SwaggerOperation("Logs out a user")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Logout succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> LogoutUser()
    {
        return await Execute(
            new LogoutUserCommand(), 
            HttpStatusCode.OK,
            nameof(LogoutUser),
            DeleteRefreshTokenCookie);
    }
}