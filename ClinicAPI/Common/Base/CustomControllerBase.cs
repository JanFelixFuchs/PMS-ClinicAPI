using System.Net;
using Application.Common.Exceptions;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Logging;
using PMS_ClinicAPI.Common.Utils.Helper;
using PMS_ClinicAPI.Common.Utils.Returns;

namespace PMS_ClinicAPI.Common.Base;

public abstract class CustomControllerBase<TController>(
    ILogger<TController> logger,
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";
    private const string RefreshTokenCookiePath = "/api/auth/refresh";
    
    protected async Task<ActionResult<HttpResult<TPayload>>> Execute<TResult, TPayload>(
        IRequest<TResult> request,
        HttpStatusCode statusCode,
        Func<TResult, TPayload> payloadSelector,
        Action<TResult>? postProcessingAction = null)
    {
        // Getting endpoint name
        var endpointName = EndpointHelper.GetEndpointName(HttpContext);
        
        // Logging endpoint execution start
        logger.LogInformation(LogMessages.EndpointCallStarted, endpointName);
        
        // Executing request
        var requestResult = await sender.Send(request);
        
        // Invoking post-processing action
        postProcessingAction?.Invoke(requestResult);

        // Logging endpoint execution completion and returning result
        var httpResult = new HttpResult<TPayload>(statusCode, payloadSelector(requestResult));
        logger.LogInformation(LogMessages.EndpointCallSucceeded, endpointName, statusCode);
        return StatusCode(httpResult.HttpStatusCode, httpResult);
    }
    
    protected async Task<ActionResult<HttpResult<EmptyPayload>>> Execute(
        IRequest request,
        HttpStatusCode statusCode,
        Action? postProcessingAction = null)
    {
        // Getting endpoint name
        var endpointName = EndpointHelper.GetEndpointName(HttpContext);
        
        // Logging endpoint executing start
        logger.LogInformation(LogMessages.EndpointCallStarted, endpointName);

        // Executing request
        await sender.Send(request);

        // Invoking post-processing action
        postProcessingAction?.Invoke();
        
        // Logging endpoint execution completion and returning result
        var httpResult = new HttpResult<EmptyPayload>(statusCode);
        logger.LogInformation(LogMessages.EndpointCallSucceeded, endpointName, statusCode);
        return StatusCode(httpResult.HttpStatusCode, httpResult);
    }

    protected string GetRefreshTokenCookie()
    {
        // Getting and checking refresh token from cookie
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
            throw new AuthorizationFailedException();
        
        // Returning refresh token
        return refreshToken;
    }
    
    protected void SetRefreshTokenCookie(string refreshToken)
    {
        // Setting refresh token cookie
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = cookieSettings.Value.Secure,
            SameSite = cookieSettings.Value.SameSiteMode,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value.RefreshTokenLifetimeInDays),
            Path = cookieSettings.Value.RestrictPath ? RefreshTokenCookiePath : null
        });
    }
    
    protected void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = cookieSettings.Value.Secure,
            SameSite = cookieSettings.Value.SameSiteMode,
            Path = cookieSettings.Value.RestrictPath ? RefreshTokenCookiePath : null
        });
    }
}