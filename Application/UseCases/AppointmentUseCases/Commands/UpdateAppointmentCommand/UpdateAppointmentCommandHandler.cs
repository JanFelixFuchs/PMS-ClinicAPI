using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.DeviceRepositories;
using Application.Repositories.PatientRepositories;
using Application.Repositories.RoomRepositories;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentUseCases.Commands.UpdateAppointmentCommand;

public class UpdateAppointmentCommandHandler(
    ILogger<UpdateAppointmentCommandHandler> logger,
    IAppointmentCategoryRepository appointmentCategoryRepository,
    IAppointmentRepository appointmentRepository,
    IClinicianRepository  clinicianRepository,
    IDeviceRepository deviceRepository,
    IPatientRepository patientRepository,
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAppointmentCommand, AppointmentDetailedOutputModel>
{
    public async Task<AppointmentDetailedOutputModel> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
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
            
            // Querying and checking appointment categories
            var appointmentCategories = await appointmentCategoryRepository.GetByClinicIdAndAppointmentCategoryIdsAsync(
                request.Clinic.Id, 
                request.AppointmentCategoryIds, 
                cancellationToken);
            var missingAppointmentCategoryIds = request.AppointmentCategoryIds.Except(appointmentCategories.Select(appointmentCategory => appointmentCategory.Id)).ToList();
            if (missingAppointmentCategoryIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(appointmentCategories), missingAppointmentCategoryIds);
                throw new NotFoundException(nameof(AppointmentCategory), missingAppointmentCategoryIds);
            }
            
            // Querying and checking patient
            var patient = await patientRepository.GetByClinicIdAndPatientIdAsync(
                request.Clinic.Id, 
                request.PatientId, 
                cancellationToken);
            if (patient == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(patient), request.PatientId);
                throw new NotFoundException(nameof(Patient), request.PatientId);
            }
            
            // Querying overlapping appointments
            var overlappingAppointments = await appointmentRepository.GetOverlappingByClinicIdAndDateTimesAsync(
                request.Clinic.Id, 
                request.StartTime, 
                request.EndTime, 
                cancellationToken,
                overlappingAppointment => overlappingAppointment.Room,
                overlappingAppointment => overlappingAppointment.Devices,
                overlappingAppointment => overlappingAppointment.Clinicians);
            overlappingAppointments = overlappingAppointments.Where(overlappingAppointment => overlappingAppointment.Id != appointment.Id).ToList();
            
            // Querying and checking room
            var room = await roomRepository.GetByClinicIdAndRoomIdAsync(
                request.Clinic.Id, 
                request.RoomId, 
                cancellationToken);
            if (room == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(room), request.RoomId);
                throw new NotFoundException(nameof(Room), request.RoomId);
            }
            
            // Checking overlapping rooms
            if (overlappingAppointments.Any(overlappingAppointment => overlappingAppointment.RoomId == request.RoomId))
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.RoomId), nameof(Appointment));
                throw new PropertyAlreadyInUseException<Guid>(nameof(Appointment), nameof(Appointment.Room), request.RoomId);
            }
            
            // Querying and checking devices
            var devices = await deviceRepository.GetByClinicIdAndDeviceIdsAsync(
                request.Clinic.Id, 
                request.DeviceIds, 
                cancellationToken);
            var missingDeviceIds = request.DeviceIds.Except(devices.Select(device => device.Id)).ToList();
            if (missingDeviceIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(devices), missingDeviceIds);
                throw new NotFoundException(nameof(Device), missingDeviceIds);
            }
            
            // Checking overlapping devices
            if (overlappingAppointments.Any(overlappingAppointment => overlappingAppointment.Devices.Any(device => request.DeviceIds.Contains(device.Id))))
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.DeviceIds), nameof(Appointment));
                throw new PropertyAlreadyInUseException<Guid>(nameof(Appointment), nameof(Appointment.Devices), request.DeviceIds);
            }
            
            // Querying and checking clinicians
            var clinicians = await clinicianRepository.GetByClinicIdAndClinicianIdsAsync(
                request.Clinic.Id,
                request.ClinicianIds,
                cancellationToken);
            var missingClinicianIds = request.ClinicianIds.Except(clinicians.Select(clinician => clinician.Id)).ToList();
            if (missingClinicianIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(clinicians), missingClinicianIds);
                throw new NotFoundException(nameof(Clinician), missingClinicianIds);
            }
            
            // Checking overlapping clinicians
            if (overlappingAppointments.Any(overlappingAppointment => overlappingAppointment.Clinicians.Any(clinician => request.ClinicianIds.Contains(clinician.Id))))
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.ClinicianIds), nameof(Appointment));
                throw new PropertyAlreadyInUseException<Guid>(nameof(Appointment), nameof(Appointment.Clinicians), request.ClinicianIds);
            }
            
            // Updating appointment
            appointment.Update(
                request.Title,
                request.StartTime, 
                request.EndTime, 
                appointmentCategories, 
                patient,
                room, 
                devices,
                clinicians);
            
            // Returning output model
            return new AppointmentDetailedOutputModel(
                appointment,
                appointmentCategories,
                patient,
                room,
                devices,
                clinicians,
                null);
        }, cancellationToken);
    }
}