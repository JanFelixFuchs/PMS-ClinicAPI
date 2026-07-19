using System.Net;
using Application.Common.OutputModels.PatientOutputModels;
using Application.UseCases.PatientUseCases.Commands.ArchivePatientCommand;
using Application.UseCases.PatientUseCases.Commands.DeletePatientCommand;
using Application.UseCases.PatientUseCases.Commands.UnarchivePatientCommand;
using Application.UseCases.PatientUseCases.Queries.ReadPatientQuery;
using Application.UseCases.PatientUseCases.Queries.ReadPatientsQuery;
using Infrastructure.Common.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Base;
using PMS_ClinicAPI.Common.InputModels.PatientInputModels;
using PMS_ClinicAPI.Common.Mappings.PatientMappings;
using PMS_ClinicAPI.Common.Utils.Returns;
using Swashbuckle.AspNetCore.Annotations;

namespace PMS_ClinicAPI.Controllers.PatientControllers;

[ApiController]
[Route("api/patients")]
public class PatientController(
    ILogger<PatientController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings, 
    ISender sender)
    : CustomControllerBase<PatientController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreatePatient)]
    [SwaggerOperation("Creates a patient")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<PatientDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<PatientDetailedOutputModel>>> CreatePatient([FromBody] CreatePatientInputModel createPatientInputModel)
    {
        return await Execute(
            createPatientInputModel.ToCreatePatientCommand(),
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadPatient)]
    [SwaggerOperation("Reads all patients")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<PatientOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<PatientOverviewOutputModel>>>> ReadPatients([FromQuery] bool archived = false)
    {
        return await Execute(
            new ReadPatientsQuery(archived),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadPatient)]
    [SwaggerOperation("Reads a patient by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<PatientDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<PatientDetailedOutputModel>>> ReadPatient([FromRoute] Guid id)
    {
        return await Execute(
            new ReadPatientQuery(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdatePatient)]
    [SwaggerOperation("Updates a patient by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<PatientDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<PatientDetailedOutputModel>>> UpdatePatient(
        [FromRoute] Guid id,
        [FromBody] UpdatePatientInputModel updatePatientInputModel)
    {
        return await Execute(
            updatePatientInputModel.ToUpdatePatientCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/archive")]
    [Authorize(Policy = PolicyDefinitions.CanArchivePatient)]
    [SwaggerOperation("Archives a patient by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Archiving succeeded", typeof(HttpResult<PatientDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<PatientDetailedOutputModel>>> ArchivePatient([FromRoute] Guid id)
    {
        return await Execute(
            new ArchivePatientCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}/unarchive")]
    [Authorize(Policy = PolicyDefinitions.CanArchivePatient)]
    [SwaggerOperation("Unarchives a patient by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Unarchiving succeeded", typeof(HttpResult<PatientDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<PatientDetailedOutputModel>>> UnarchivePatient([FromRoute] Guid id)
    {
        return await Execute(
            new UnarchivePatientCommand(id),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeletePatient)]
    [SwaggerOperation("Deletes a patient by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeletePatient([FromRoute] Guid id)
    {
        return await Execute(new DeletePatientCommand(id), HttpStatusCode.OK);
    }
}