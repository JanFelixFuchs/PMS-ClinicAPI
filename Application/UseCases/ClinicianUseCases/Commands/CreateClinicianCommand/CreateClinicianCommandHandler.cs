using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianUseCases.Commands.CreateClinicianCommand;

public class CreateClinicianCommandHandler(
    ILogger<CreateClinicianCommandHandler> logger,
    IClinicianCategoryRepository clinicianCategoryRepository,
    IClinicianRepository clinicianRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateClinicianCommand, ClinicianDetailedOutputModel>
{
    public async Task<ClinicianDetailedOutputModel> Handle(CreateClinicianCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking clinician categories
            var clinicianCategories = await clinicianCategoryRepository.GetByClinicIdAndClinicianCategoryIdsAsync(
                request.Clinic.Id, 
                request.ClinicianCategoryIds, 
                cancellationToken);
            var missingClinicianCategoryIds = request.ClinicianCategoryIds.Except(clinicianCategories.Select(clinicianCategory => clinicianCategory.Id)).ToList();
            if (missingClinicianCategoryIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(clinicianCategories), missingClinicianCategoryIds);
                throw new NotFoundException(nameof(ClinicianCategory), missingClinicianCategoryIds);
            }
            
            // Creating clinician
            var clinician = new Clinician(
                request.Clinic,
                request.FirstName,
                request.LastName,
                clinicianCategories);

            // Adding clinician
            await clinicianRepository.AddAsync(clinician, cancellationToken);
            
            // Returning output model
            return new ClinicianDetailedOutputModel(
                clinician, 
                clinician.ClinicianCategories, 
                clinician.Appointments, 
                clinician.AppointmentProtocols, 
                clinician.Results);
        }, cancellationToken);
    }
}