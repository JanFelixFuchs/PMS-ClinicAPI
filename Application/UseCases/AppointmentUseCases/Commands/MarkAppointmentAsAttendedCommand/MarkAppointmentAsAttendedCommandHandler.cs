using Application.Common.Exceptions;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Providers;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;

namespace Application.UseCases.AppointmentUseCases.Commands.MarkAppointmentAsAttendedCommand;

public class MarkAppointmentAsAttendedCommandHandler(
    IAppointmentProtocolRepository appointmentProtocolRepository,
    IAppointmentRepository appointmentRepository,
    IDateTimeProvider dateTimeProvider,
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
                throw new NotFoundException(nameof(Appointment), request.Id);
            
            // Marking appointment as attended
            appointment.MarkAsAttended(dateTimeProvider.UtcNow);
            
            // Creating appointment protocol
            var appointmentProtocol = new AppointmentProtocol(
                request.Clinic,
                appointment,
                dateTimeProvider.UtcNow);
            
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