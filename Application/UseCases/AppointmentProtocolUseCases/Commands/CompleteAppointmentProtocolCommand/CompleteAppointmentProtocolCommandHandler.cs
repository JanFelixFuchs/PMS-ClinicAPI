using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentProtocolUseCases.Commands.CompleteAppointmentProtocolCommand;

public class CompleteAppointmentProtocolCommandHandler(
    ILogger<CompleteAppointmentProtocolCommandHandler> logger,
    IAppointmentProtocolRepository appointmentProtocolRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CompleteAppointmentProtocolCommand, AppointmentProtocolDetailedOutputModel>
{
    public async Task<AppointmentProtocolDetailedOutputModel> Handle(CompleteAppointmentProtocolCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking appointment protocol
            var appointmentProtocol = await appointmentProtocolRepository.GetByClinicIdAndAppointmentProtocolIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                appointmentProtocol => appointmentProtocol.Appointment,
                appointmentProtocol => appointmentProtocol.Patient,
                appointmentProtocol => appointmentProtocol.Clinician,
                appointmentProtocol => appointmentProtocol.Room,
                appointmentProtocol => appointmentProtocol.Devices);
            if (appointmentProtocol == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(appointmentProtocol), request.Id);
                throw new NotFoundException(nameof(AppointmentProtocol), request.Id);
            }
            
            // Completing appointment protocol
            appointmentProtocol.Complete();
            
            // Returning output model
            return new AppointmentProtocolDetailedOutputModel(
                appointmentProtocol,
                appointmentProtocol.Appointment,
                appointmentProtocol.Patient,
                appointmentProtocol.Clinician,
                appointmentProtocol.Room,
                appointmentProtocol.Devices);
        }, cancellationToken);
    }
}