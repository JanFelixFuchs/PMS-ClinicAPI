using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentUseCases.Commands.MarkAppointmentAsAttendedCommand;

public class MarkAppointmentAsAttendedCommandHandler(
    ILogger<MarkAppointmentAsAttendedCommandHandler> logger,
    IAppointmentProtocolRepository appointmentProtocolRepository,
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<MarkAppointmentAsAttendedCommand, AppointmentDetailedOutputModel> 
{
    public async Task<AppointmentDetailedOutputModel> Handle(MarkAppointmentAsAttendedCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking appointment
            var appointment = await appointmentRepository.GetByClinicIdAndAppointmentIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                appointment => appointment.AppointmentCategories,
                appointment => appointment.Patient,
                appointment => appointment.Room,
                appointment => appointment.Devices,
                appointment => appointment.Clinicians);
            if (appointment == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(appointment), request.Id);
                throw new NotFoundException(nameof(Appointment), request.Id);
            }
            
            // Marking appointment as attended
            appointment.MarkAsAttended();
            
            // Creating appointment protocol
            var appointmentProtocol = new AppointmentProtocol(
                request.Clinic,
                appointment);
            
            // Saving appointment protocol
            await appointmentProtocolRepository.AddAsync(appointmentProtocol, cancellationToken);
            
            // Returning output model
            return new AppointmentDetailedOutputModel(
                appointment,
                appointment.AppointmentCategories,
                appointment.Patient,
                appointment.Room,
                appointment.Devices,
                appointment.Clinicians,
                appointmentProtocol);
        }, cancellationToken);
    }
}