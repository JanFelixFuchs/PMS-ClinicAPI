using Application.Common.Exceptions;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Commands.UpdateClinicianCommand;

public class UpdateClinicianCommandHandler(
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
                throw new NotFoundException(nameof(Clinician), request.Id);
            
            // Querying and checking clinician categories
            var clinicianCategories = await clinicianCategoryRepository.GetByClinicIdAndClinicianCategoryIdsAsync(
                request.Clinic.Id, 
                request.ClinicianCategoryIds, 
                cancellationToken);
            var missingClinicianCategoryIds = request.ClinicianCategoryIds.Except(clinicianCategories.Select(clinicianCategory => clinicianCategory.Id)).ToList();
            if (missingClinicianCategoryIds.Count > 0)
                throw new NotFoundException(nameof(ClinicianCategory), missingClinicianCategoryIds);
            
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