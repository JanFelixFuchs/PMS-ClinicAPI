using System.Net;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.UseCases.ResultUseCases.Commands.DeleteResultCommand;
using Application.UseCases.ResultUseCases.Queries.ReadResultQuery;
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
[Route("api/results")]
public class ResultController(
    ILogger<ResultController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<ResultController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateResult)]
    [SwaggerOperation("Creates a result")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<ResultDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ResultDetailedOutputModel>>> CreateResult([FromBody] CreateResultInputModel createResultInputModel)
    {
        return await Execute(
            createResultInputModel.ToCreateResultCommand(),
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadResult)]
    [SwaggerOperation("Reads a result by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<ResultDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ResultDetailedOutputModel>>> ReadResult([FromRoute] Guid id)
    {
        return await Execute(
            new ReadResultQuery(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteResult)]
    [SwaggerOperation("Deletes a result by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteResult([FromRoute] Guid id)
    {
        return await Execute(new DeleteResultCommand(id), HttpStatusCode.OK);
    }
}