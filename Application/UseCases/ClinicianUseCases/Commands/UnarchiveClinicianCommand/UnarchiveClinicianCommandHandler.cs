using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianUseCases.Commands.UnarchiveClinicianCommand;

public class UnarchiveClinicianCommandHandler(
    ILogger<UnarchiveClinicianCommandHandler> logger,
    IClinicianRepository clinicianRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UnarchiveClinicianCommand, ClinicianDetailedOutputModel>
{
    public async Task<ClinicianDetailedOutputModel> Handle(UnarchiveClinicianCommand request, CancellationToken cancellationToken)
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
            
            // Unarchiving clinician
            clinician.Unarchive();
            
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