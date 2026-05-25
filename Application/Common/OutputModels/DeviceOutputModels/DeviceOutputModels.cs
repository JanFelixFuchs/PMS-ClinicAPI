using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.DeviceEntities;

namespace Application.Common.OutputModels.DeviceOutputModels;

public class DeviceOverviewOutputModel(Device device)
{
    public Guid Id { get; init; } = device.Id;
    public string Name { get; init; } = device.Name;
    public string Abbreviation { get; init; } = device.Abbreviation;
    public string SerialNumber { get; init; } = device.SerialNumber;
    public DeviceStatus Status { get; init; } = device.Status;
    public string Producer { get; init; } = device.Producer;
    public bool IsArchived { get; init; } = device.IsArchived;
}

public class DeviceDetailedOutputModel(
    Device device,
    ICollection<DeviceCategory> deviceCategories,
    ICollection<Appointment> appointments, 
    ICollection<AppointmentProtocol> appointmentProtocols,
    ICollection<Result> results) 
    : DeviceOverviewOutputModel(device)
{
    public ICollection<DeviceCategoryOverviewOutputModel> DeviceCategories { get; init; } = deviceCategories.Select(deviceCategory => new DeviceCategoryOverviewOutputModel(deviceCategory)).ToList();
    public DateTime? DateOfPurchase { get; init; } = device.DateOfPurchase;
    public DateTime? DateOfLastMaintenance { get; init; } = device.DateOfLastMaintenance;
    public ICollection<AppointmentOverviewOutputModel> Appointments { get; init; } = appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)).ToList();
    public ICollection<AppointmentProtocolOverviewOutputModel> AppointmentProtocols { get; init; } = appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)).ToList();
    public ICollection<ResultOverviewOutputModel> Results { get; init; } = results.Select(result => new ResultOverviewOutputModel(result)).ToList();
}