using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;
using LogMessages = Application.Common.Logging.LogMessages;

namespace Application.UseCases.AppointmentUseCases.Commands.DeleteAppointmentCommand;

public class DeleteAppointmentCommandHandler(
    ILogger<DeleteAppointmentCommandHandler> logger,
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteAppointmentCommand>
{
    public async Task Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking appointment
            var appointment = await appointmentRepository.GetByClinicIdAndAppointmentIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken);
            if (appointment == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(appointment), request.Id);
                throw new NotFoundException(nameof(Appointment), request.Id);
            }
            
            // Deleting appointment
            appointment.Delete();
        }, cancellationToken);
    }
}