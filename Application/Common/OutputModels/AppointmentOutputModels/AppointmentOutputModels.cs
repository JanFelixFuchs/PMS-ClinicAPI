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

public class AppointmentOverviewOutputModel(Appointment appointment)
{
    public Guid Id { get; init; } = appointment.Id;
    public string Title { get; init; } = appointment.Title;
    public DateTime StartTime { get; init; } = appointment.StartTime;
    public DateTime EndTime { get; init; } = appointment.EndTime;
    public AppointmentStatus Status { get; init; } = appointment.Status;
}

public class AppointmentDetailedOutputModel(
    Appointment appointment,
    ICollection<AppointmentCategory> appointmentCategories,
    Patient patient, 
    Room room,
    ICollection<Device> devices,
    ICollection<Clinician> clinicians,
    AppointmentProtocol? appointmentProtocol)
    : AppointmentOverviewOutputModel(appointment) 
{
    public ICollection<AppointmentCategoryOverviewOutputModel> AppointmentCategories { get; init; } = appointmentCategories.Select(appointmentCategory => new AppointmentCategoryOverviewOutputModel(appointmentCategory)).ToList();
    public PatientOverviewOutputModel Patient { get; init; } = new(patient);
    public RoomOverviewOutputModel Room { get; init; } = new(room);
    public ICollection<DeviceOverviewOutputModel> Devices { get; init; } = devices.Select(device => new DeviceOverviewOutputModel(device)).ToList();
    public ICollection<ClinicianOverviewOutputModel> Clinicians { get; init; } = clinicians.Select(clinician => new ClinicianOverviewOutputModel(clinician)).ToList();
    public AppointmentProtocolOverviewOutputModel? AppointmentProtocol { get; init; } = appointmentProtocol != null ? new AppointmentProtocolOverviewOutputModel(appointmentProtocol) : null;
}