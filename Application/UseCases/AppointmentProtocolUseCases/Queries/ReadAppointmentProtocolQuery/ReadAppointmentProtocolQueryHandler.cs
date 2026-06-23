using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentProtocolUseCases.Queries.ReadAppointmentProtocolQuery;

public class ReadAppointmentProtocolQueryHandler(
    ILogger<ReadAppointmentProtocolQueryHandler> logger,
    IAppointmentProtocolRepository appointmentProtocolRepository) 
    : IRequestHandler<ReadAppointmentProtocolQuery, AppointmentProtocolDetailedOutputModel>
{
    public async Task<AppointmentProtocolDetailedOutputModel> Handle(ReadAppointmentProtocolQuery request, CancellationToken cancellationToken)
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
        
        // Returning output model
        return new AppointmentProtocolDetailedOutputModel(
            appointmentProtocol,
            appointmentProtocol.Appointment,
            appointmentProtocol.Patient,
            appointmentProtocol.Clinician,
            appointmentProtocol.Room,
            appointmentProtocol.Devices);
    }
}