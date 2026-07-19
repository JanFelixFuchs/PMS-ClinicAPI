using System.Net;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.UseCases.ClinicianUseCases.Commands.ArchiveClinicianCommand;
using Application.UseCases.ClinicianUseCases.Commands.DeleteClinicianCommand;
using Application.UseCases.ClinicianUseCases.Commands.UnarchiveClinicianCommand;
using Application.UseCases.ClinicianUseCases.Queries.ReadClinicianQuery;
using Application.UseCases.ClinicianUseCases.Queries.ReadCliniciansQuery;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.ClinicianInputModels;
using PMS_ClinicAPI.Common.Mappings.ClinicianMappings;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.ClinicianControllers;

[ApiController]
[Route("api/clinicians")]
public class ClinicianController(
    ILogger<ClinicianController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<ClinicianController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateClinician)]
    [SwaggerOperation("Creates a clinician")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<ClinicianDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianDetailedOutputModel>>> CreateClinician([FromBody] CreateClinicianInputModel createClinicianInputModel)
    {
        return await Execute(
            createClinicianInputModel.ToCreateClinicianCommand(), 
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadClinician)]
    [SwaggerOperation("Reads all clinicians")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<ClinicianOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<ClinicianOverviewOutputModel>>>> ReadClinicians([FromQuery] bool archived = false)
    {
        return await Execute(
            new ReadCliniciansQuery(archived),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadClinician)]
    [SwaggerOperation("Reads a clinician by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<ClinicianDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianDetailedOutputModel>>> ReadClinician([FromRoute] Guid id)
    {
        return await Execute(
            new ReadClinicianQuery(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateClinician)]
    [SwaggerOperation("Updates a clinician by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<ClinicianDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianDetailedOutputModel>>> UpdateClinician(
        [FromRoute] Guid id,
        [FromBody] UpdateClinicianInputModel updateClinicianInputModel)
    {
        return await Execute(
            updateClinicianInputModel.ToUpdateClinicianCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/archive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveClinician)]
    [Authorize(Policy = PolicyDefinitions.CanArchiveUser)]
    [SwaggerOperation("Archives a clinician by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Archiving succeeded", typeof(HttpResult<ClinicianDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianDetailedOutputModel>>> ArchiveClinician([FromRoute] Guid id)
    {
        return await Execute(
            new ArchiveClinicianCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/unarchive")]
    [Authorize(Policy = PolicyDefinitions.CanArchiveClinician)]
    [SwaggerOperation("Unarchives a clinician by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Unarchiving succeeded", typeof(HttpResult<ClinicianDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianDetailedOutputModel>>> UnarchiveClinician([FromRoute] Guid id)
    {
        return await Execute(
            new UnarchiveClinicianCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteClinician)]
    [SwaggerOperation("Deletes a clinician by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteClinician([FromRoute] Guid id)
    {
        return await Execute(new DeleteClinicianCommand(id), HttpStatusCode.OK);
    }
}