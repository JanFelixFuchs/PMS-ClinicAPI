using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianUseCases.Commands.DeleteClinicianCommand;

public class DeleteClinicianCommandHandler(
    ILogger<DeleteClinicianCommandHandler> logger,
    IClinicianRepository clinicianRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteClinicianCommand>
{
    public async Task Handle(DeleteClinicianCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking clinician
            var clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                clinician => clinician.User,
                clinician => clinician.Appointments,
                clinician => clinician.AppointmentProtocols,
                clinician => clinician.Results);
            if (clinician == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(clinician), request.Id);
                throw new NotFoundException(nameof(Clinician), request.Id);
            }
            
            // Deleting clinician
            clinician.Delete(clinician.User, clinician.Appointments, clinician.AppointmentProtocols, clinician.Results);
        }, cancellationToken);
    }
}