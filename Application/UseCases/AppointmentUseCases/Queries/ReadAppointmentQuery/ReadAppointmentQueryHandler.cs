using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentQuery;

public class ReadAppointmentQueryHandler(
    ILogger<ReadAppointmentQueryHandler> logger,
    IAppointmentRepository appointmentRepository) 
    : IRequestHandler<ReadAppointmentQuery, AppointmentDetailedOutputModel>
{
    public async Task<AppointmentDetailedOutputModel> Handle(ReadAppointmentQuery request, CancellationToken cancellationToken)
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
            appointment => appointment.Clinicians,
            appointment => appointment.AppointmentProtocol);
        if (appointment == null)
        {
            logger.LogWarning(LogMessages.EntityNotFound, nameof(appointment), request.Id);
            throw new NotFoundException(nameof(Appointment), request.Id);
        }
        
        // Returning output model
        return new AppointmentDetailedOutputModel(
            appointment,
            appointment.AppointmentCategories,
            appointment.Patient,
            appointment.Room,
            appointment.Devices,
            appointment.Clinicians,
            appointment.AppointmentProtocol);
    }
}