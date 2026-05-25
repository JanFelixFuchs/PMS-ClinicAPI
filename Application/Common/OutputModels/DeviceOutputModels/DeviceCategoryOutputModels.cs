using Domain.Entities.DeviceEntities;

namespace Application.Common.OutputModels.DeviceOutputModels;

public class DeviceCategoryOverviewOutputModel(DeviceCategory deviceCategory)
{
    public Guid Id { get; init; } = deviceCategory.Id;
    public string Name { get; init; } = deviceCategory.Name;
    public string Abbreviation { get; init; } = deviceCategory.Abbreviation;
    public string Color { get; init; } = deviceCategory.Color;
}

public class DeviceCategoryDetailedOutputModel(
    DeviceCategory deviceCategory,
    ICollection<Device> devices) 
    : DeviceCategoryOverviewOutputModel(deviceCategory)
{
    public ICollection<DeviceOverviewOutputModel> Devices { get; init; } = devices.Select(device => new DeviceOverviewOutputModel(device)).ToList();
}