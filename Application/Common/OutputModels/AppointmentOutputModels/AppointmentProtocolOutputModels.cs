using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.OutputModels.RoomOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;

namespace Application.Common.OutputModels.AppointmentOutputModels;

public class AppointmentProtocolOverviewOutputModel(AppointmentProtocol appointmentProtocol)
{
    public Guid Id { get; init; } = appointmentProtocol.Id;
    public DateTime DateOfAppointment { get; init; } = appointmentProtocol.DateOfAppointment;
    public AppointmentProtocolStatus Status { get; init; } = appointmentProtocol.Status;
}

public class AppointmentProtocolDetailedOutputModel(
    AppointmentProtocol appointmentProtocol,
    Appointment appointment,
    Patient patient, 
    Clinician clinician, 
    Room room, 
    ICollection<Device> devices)
    : AppointmentProtocolOverviewOutputModel(appointmentProtocol)
{
    public DateTime? DateOfProcessingStart { get; init; } = appointmentProtocol.DateOfProcessingStart;
    public DateTime? DateOfProcessingCompletion { get; init; } = appointmentProtocol.DateOfProcessingCompletion;
    public string? Symptoms { get; init; } = appointmentProtocol.Symptoms;
    public string? Diagnosis { get; init; } = appointmentProtocol.Diagnosis;
    public string? Treatment { get; init; } = appointmentProtocol.Treatment;
    public string? Remarks { get; init; } = appointmentProtocol.Remarks;
    public AppointmentOverviewOutputModel Appointment { get; init; } = new(appointment);
    public PatientOverviewOutputModel Patient { get; init; } = new(patient);
    public ClinicianOverviewOutputModel Clinician { get; init; } = new(clinician);
    public RoomOverviewOutputModel Room { get; init; } = new(room);
    public ICollection<DeviceOverviewOutputModel> Devices { get; init; } = devices.Select(device => new DeviceOverviewOutputModel(device)).ToList();
}