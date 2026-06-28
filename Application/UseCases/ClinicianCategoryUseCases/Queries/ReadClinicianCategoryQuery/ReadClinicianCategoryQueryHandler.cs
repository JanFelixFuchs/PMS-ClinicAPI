using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianCategoryUseCases.Queries.ReadClinicianCategoryQuery;

public class ReadClinicianCategoryQueryHandler(
    ILogger<ReadClinicianCategoryQueryHandler> logger,
    IClinicianCategoryRepository clinicianCategoryRepository) 
    : IRequestHandler<ReadClinicianCategoryQuery, ClinicianCategoryDetailedOutputModel>
{
    public async Task<ClinicianCategoryDetailedOutputModel> Handle(ReadClinicianCategoryQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking clinician category
        var clinicianCategory = await clinicianCategoryRepository.GetByClinicIdAndClinicianCategoryIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken, 
            clinicianCategory => clinicianCategory.Clinicians);
        if (clinicianCategory == null)
        {
            logger.LogWarning(LogMessages.EntityNotFound, nameof(clinicianCategory), request.Id);
            throw new NotFoundException(nameof(ClinicianCategory), request.Id);
        } 
        
        // Returning output model
        return new ClinicianCategoryDetailedOutputModel(clinicianCategory, clinicianCategory.Clinicians);
    }
}