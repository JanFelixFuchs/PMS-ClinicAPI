using Application.UseCases.DeviceUseCases.Commands.CreateDeviceCommand;
using Application.UseCases.DeviceUseCases.Commands.UpdateDeviceCommand;
using PMS_ClinicAPI.Common.InputModels.DeviceInputModels;

namespace PMS_ClinicAPI.Common.Mappings.DeviceMappings;

public static class DeviceMappings
{
    public static CreateDeviceCommand ToCreateDeviceCommand(this CreateDeviceInputModel createDeviceInputModel)
    {
        return new CreateDeviceCommand(
            createDeviceInputModel.Name,
            createDeviceInputModel.Abbreviation,
            createDeviceInputModel.SerialNumber,
            createDeviceInputModel.Status!.Value,
            createDeviceInputModel.Producer,
            createDeviceInputModel.DeviceCategoryIds,
            createDeviceInputModel.DateOfPurchase,
            createDeviceInputModel.DateOfLastMaintenance);
    }
    
    public static UpdateDeviceCommand ToUpdateDeviceCommand(
        this UpdateDeviceInputModel updateDeviceInputModel,
        Guid id)
    {
        return new UpdateDeviceCommand(
            id,
            updateDeviceInputModel.Name,
            updateDeviceInputModel.Abbreviation,
            updateDeviceInputModel.DeviceCategoryIds,
            updateDeviceInputModel.DateOfLastMaintenance);
    }
}