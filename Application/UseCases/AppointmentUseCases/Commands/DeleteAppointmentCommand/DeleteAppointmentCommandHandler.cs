using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;

namespace Application.UseCases.AppointmentUseCases.Commands.DeleteAppointmentCommand;

public class DeleteAppointmentCommandHandler(
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
                throw new NotFoundException(nameof(Appointment), request.Id);
            
            // Deleting appointment
            appointment.Delete();
        }, cancellationToken);
    }
}