using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianUseCases.Commands.UpdateClinicianCommand;

public class UpdateClinicianCommandHandler(
    ILogger<UpdateClinicianCommandHandler> logger,
    IClinicianCategoryRepository clinicianCategoryRepository,
    IClinicianRepository clinicianRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateClinicianCommand, ClinicianDetailedOutputModel>
{
    public async Task<ClinicianDetailedOutputModel> Handle(UpdateClinicianCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking clinician
            var clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                request.Clinic.Id,
                request.Id,
                cancellationToken,
                clinician => clinician.ClinicianCategories,
                clinician => clinician.Appointments,
                clinician => clinician.AppointmentProtocols,
                clinician => clinician.Results);
            if (clinician == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(clinician), request.Id);
                throw new NotFoundException(nameof(Clinician), request.Id);
            }
            
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
            
            // Updating clinician
            clinician.Update(request.LastName, clinicianCategories);
            
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