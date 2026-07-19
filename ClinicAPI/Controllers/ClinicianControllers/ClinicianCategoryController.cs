using System.Net;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.UseCases.ClinicianCategoryUseCases.Commands.DeleteClinicianCategoryCommand;
using Application.UseCases.ClinicianCategoryUseCases.Queries.ReadClinicianCategoriesQuery;
using Application.UseCases.ClinicianCategoryUseCases.Queries.ReadClinicianCategoryQuery;
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
[Route("api/clinicianCategories")]
public class ClinicianCategoryController(
    ILogger<ClinicianCategoryController> logger, 
    IOptions<CookieSettings> cookieSettings,
    IOptions<JwtSettings> jwtSettings,
    ISender sender)
    : CustomControllerBase<ClinicianCategoryController>(logger, cookieSettings, jwtSettings, sender)
{
    [HttpPost]
    [Authorize(Policy = PolicyDefinitions.CanCreateClinicianCategory)]
    [SwaggerOperation("Creates a clinician category")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Creating succeeded", typeof(HttpResult<ClinicianCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianCategoryDetailedOutputModel>>> CreateClinicianCategory([FromBody] CreateClinicianCategoryInputModel createClinicianCategoryInputModel)
    {
        return await Execute(
            createClinicianCategoryInputModel.ToCreateClinicianCategoryCommand(),
            HttpStatusCode.Created,
            payloadSelector: result => result);
    }
    
    [HttpGet]
    [Authorize(Policy = PolicyDefinitions.CanReadClinicianCategory)]
    [SwaggerOperation("Reads all clinician categories")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<List<ClinicianCategoryOverviewOutputModel>>))]
    public async Task<ActionResult<HttpResult<List<ClinicianCategoryOverviewOutputModel>>>> ReadClinicianCategories()
    {
        return await Execute(
            new ReadClinicianCategoriesQuery(),
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanReadClinicianCategory)]
    [SwaggerOperation("Reads a clinician category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Reading succeeded", typeof(HttpResult<ClinicianCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianCategoryDetailedOutputModel>>> ReadClinicianCategory([FromRoute] Guid id)
    {
        return await Execute(
            new ReadClinicianCategoryQuery(id), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanUpdateClinicianCategory)]
    [SwaggerOperation("Updates a clinician category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Updating succeeded", typeof(HttpResult<ClinicianCategoryDetailedOutputModel>))]
    public async Task<ActionResult<HttpResult<ClinicianCategoryDetailedOutputModel>>> UpdateClinicianCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateClinicianCategoryInputModel updateClinicianCategoryInputModel)
    {
        return await Execute(
            updateClinicianCategoryInputModel.ToUpdateClinicianCategoryCommand(id), 
            HttpStatusCode.OK,
            payloadSelector: result => result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyDefinitions.CanDeleteClinicianCategory)]
    [SwaggerOperation("Deletes a clinician category by its id")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Deleting succeeded", typeof(HttpResult<EmptyPayload>))]
    public async Task<ActionResult<HttpResult<EmptyPayload>>> DeleteClinicianCategory([FromRoute] Guid id)
    {
        return await Execute(new DeleteClinicianCategoryCommand(id), HttpStatusCode.OK);
    }
}