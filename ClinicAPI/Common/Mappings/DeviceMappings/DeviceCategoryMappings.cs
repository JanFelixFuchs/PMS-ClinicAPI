using Application.UseCases.DeviceCategoryUseCases.Commands.CreateDeviceCategoryCommand;
using Application.UseCases.DeviceCategoryUseCases.Commands.UpdateDeviceCategoryCommand;
using PMS_ClinicAPI.Common.InputModels.DeviceInputModels;

namespace PMS_ClinicAPI.Common.Mappings.DeviceMappings;

public static class DeviceCategoryMappings
{
    public static CreateDeviceCategoryCommand ToCreateDeviceCategoryCommand(this CreateDeviceCategoryInputModel createDeviceCategoryInputModel)
    {
        return new CreateDeviceCategoryCommand(
            createDeviceCategoryInputModel.Name,
            createDeviceCategoryInputModel.Abbreviation, 
            createDeviceCategoryInputModel.Color,
            createDeviceCategoryInputModel.DeviceIds);
    }
    
    public static UpdateDeviceCategoryCommand ToUpdateDeviceCategoryCommand(
        this UpdateDeviceCategoryInputModel updateDeviceCategoryInputModel,
        Guid id)
    {
        return new UpdateDeviceCategoryCommand(
            id,
            updateDeviceCategoryInputModel.Name,
            updateDeviceCategoryInputModel.Abbreviation,
            updateDeviceCategoryInputModel.Color,
            updateDeviceCategoryInputModel.DeviceIds);
    }
}