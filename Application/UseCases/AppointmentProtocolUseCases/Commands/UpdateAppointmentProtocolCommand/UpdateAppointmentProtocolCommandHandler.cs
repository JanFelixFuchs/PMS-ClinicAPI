using Application.Common.Exceptions;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.DeviceRepositories;
using Application.Repositories.RoomRepositories;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.AppointmentProtocolUseCases.Commands.UpdateAppointmentProtocolCommand;

public class UpdateAppointmentProtocolCommandHandler(
    IAppointmentProtocolRepository appointmentProtocolRepository,
    IClinicianRepository clinicianRepository,
    IRoomRepository roomRepository,
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAppointmentProtocolCommand, AppointmentProtocolDetailedOutputModel>
{
    public async Task<AppointmentProtocolDetailedOutputModel> Handle(UpdateAppointmentProtocolCommand request, CancellationToken cancellationToken)
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
                throw new NotFoundException(nameof(AppointmentProtocol), request.Id);
            
            // Querying and checking clinician
            var clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                request.Clinic.Id,
                request.ClinicianId,
                cancellationToken);
            if (clinician == null)
                throw new NotFoundException(nameof(Clinician), request.ClinicianId);
            
            // Querying and checking room
            var room = await roomRepository.GetByClinicIdAndRoomIdAsync(
                request.Clinic.Id, 
                request.RoomId, 
                cancellationToken);
            if (room == null)
                throw new NotFoundException(nameof(Room), request.RoomId);
            
            // Querying and checking devices
            var devices = await deviceRepository.GetByClinicIdAndDeviceIdsAsync(
                request.Clinic.Id, 
                request.DeviceIds, 
                cancellationToken);
            var missingDeviceIds = request.DeviceIds.Except(devices.Select(device => device.Id)).ToList();
            if (missingDeviceIds.Count > 0)
                throw new NotFoundException(nameof(Device), missingDeviceIds);
            
            // Updating appointment protocol
            appointmentProtocol.Update(
                request.Symptoms,
                request.Diagnosis,
                request.Treatment,
                request.Remarks,
                clinician,
                room,
                devices);
            
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