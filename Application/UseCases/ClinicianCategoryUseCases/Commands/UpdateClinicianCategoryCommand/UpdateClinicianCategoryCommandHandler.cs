using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianCategoryUseCases.Commands.UpdateClinicianCategoryCommand;

public class UpdateClinicianCategoryCommandHandler(
    ILogger<UpdateClinicianCategoryCommandHandler> logger,
    IClinicianRepository clinicianRepository,
    IClinicianCategoryRepository clinicianCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateClinicianCategoryCommand, ClinicianCategoryDetailedOutputModel>
{
    public async Task<ClinicianCategoryDetailedOutputModel> Handle(UpdateClinicianCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
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
            
            // Updating clinician category
            clinicianCategory.Update(request.Name, request.Abbreviation, request.Color);
   
            // Defining clinician category
            var currentClinicians = clinicianCategory.Clinicians.ToList();
            var currentClinicianIds = currentClinicians.Select(clinician => clinician.Id).ToHashSet();
            
            // Calculating unchanged clinicians
            var unchangedClinicians = currentClinicians.Where(clinician => request.ClinicianIds.Contains(clinician.Id)).ToList();
            
            // Calculating clinicians to be changed
            var clinicianIdsToRemoveFrom = currentClinicianIds.Except(request.ClinicianIds).ToHashSet();
            var clinicianIdsToAddTo = request.ClinicianIds.Except(currentClinicianIds).ToHashSet();
            
            // Removing clinician categories from non-assigned clinicians
            var cliniciansToRemoveFrom = currentClinicians.Where(clinician => clinicianIdsToRemoveFrom.Contains(clinician.Id)).ToList();
            foreach (var clinician in cliniciansToRemoveFrom)
                clinician.RemoveClinicianCategory(clinicianCategory);

            // Adding clinician categories to newly assigned clinicians
            ICollection<Clinician> cliniciansToAddTo = new List<Clinician>();
            if (clinicianIdsToAddTo.Count > 0)
            {
                cliniciansToAddTo = await clinicianRepository.GetByClinicIdAndClinicianIdsAsync(
                    request.Clinic.Id,
                    clinicianIdsToAddTo,
                    cancellationToken);
                var missingClinicianIds = clinicianIdsToAddTo.Except(cliniciansToAddTo.Select(clinician => clinician.Id)).ToList();
                if (missingClinicianIds.Count > 0)
                {
                    logger.LogWarning(LogMessages.EntitiesNotFound, nameof(cliniciansToAddTo), missingClinicianIds);
                    throw new NotFoundException(nameof(Clinician), missingClinicianIds);
                }
        
                foreach (var clinician in cliniciansToAddTo)
                    clinician.AddClinicianCategory(clinicianCategory);
            }
            
            // Returning output model
            return new ClinicianCategoryDetailedOutputModel(clinicianCategory, unchangedClinicians.Concat(cliniciansToAddTo).ToList());
        }, cancellationToken);
    }
}