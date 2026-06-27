using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianCategoryUseCases.Commands.CreateClinicianCategoryCommand;

public class CreateClinicianCategoryCommandHandler(
    ILogger<CreateClinicianCategoryCommandHandler> logger,
    IClinicianRepository clinicianRepository,
    IClinicianCategoryRepository clinicianCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateClinicianCategoryCommand, ClinicianCategoryDetailedOutputModel>
{
    public async Task<ClinicianCategoryDetailedOutputModel> Handle(CreateClinicianCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Creating clinician category
            var clinicianCategory = new ClinicianCategory(
                request.Clinic, 
                request.Name, 
                request.Abbreviation, 
                request.Color);
            
            // Adding clinician category
            await clinicianCategoryRepository.AddAsync(clinicianCategory, cancellationToken);
            
            // Querying and checking clinicians
            var clinicians = await clinicianRepository.GetByClinicIdAndClinicianIdsAsync(
                request.Clinic.Id, 
                request.ClinicianIds, 
                cancellationToken);
            var missingClinicianIds = request.ClinicianIds.Except(clinicians.Select(clinician => clinician.Id)).ToList();
            if (missingClinicianIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(clinicians), missingClinicianIds);
                throw new NotFoundException(nameof(Clinician), missingClinicianIds);
            }
            
            // Adding clinician category to clinicians
            foreach (var clinician in clinicians)
                clinician.AddClinicianCategory(clinicianCategory);
            
            // Returning output model
            return new ClinicianCategoryDetailedOutputModel(clinicianCategory, clinicianCategory.Clinicians);
        }, cancellationToken);
    }
}