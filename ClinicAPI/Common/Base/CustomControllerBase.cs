using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS_ClinicAPI.Common.Logging;
using PMS_ClinicAPI.Common.Utils;

namespace PMS_ClinicAPI.Common.Base;

public abstract class CustomControllerBase<TController>(
    ILogger<TController> logger,
    ISender sender)
    : ControllerBase
{
    protected async Task<ActionResult<HttpResult<T>>> Execute<T>(
        IRequest<T> request,
        HttpStatusCode statusCode,
        string endpointName)
    {
        // Logging endpoint execution start
        logger.LogInformation(LogMessages.EndpointCallStarted, endpointName);
        
        // Executing request
        var requestResult = await sender.Send(request);

        // Logging endpoint execution completion and returning result
        var httpResult = new HttpResult<T>(statusCode, requestResult);
        logger.LogInformation(LogMessages.EndpointCallSucceeded, endpointName, statusCode);
        return StatusCode(httpResult.HttpStatusCode, httpResult);
    }
    
    protected async Task<ActionResult<HttpResult<EmptyPayload>>> Execute(
        IRequest request,
        string endpointName)
    {
        // Logging endpoint executing start
        logger.LogInformation(LogMessages.EndpointCallStarted, endpointName);

        // Executing request
        await sender.Send(request);

        // Logging endpoint execution completion and returning result
        var httpResult = new HttpResult<EmptyPayload>(HttpStatusCode.OK);
        logger.LogInformation(LogMessages.EndpointCallSucceeded, endpointName, HttpStatusCode.OK);
        return StatusCode(httpResult.HttpStatusCode, httpResult);
    }
}