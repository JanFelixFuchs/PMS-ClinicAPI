using Application.UseCases.ClinicianUseCases.Commands.CreateClinicianCommand;
using Application.UseCases.ClinicianUseCases.Commands.UpdateClinicianCommand;
using PMS_ClinicAPI.Common.InputModels.ClinicianInputModels;

namespace PMS_ClinicAPI.Common.Mappings.ClinicianMappings;

public static class ClinicianMappings
{
    public static CreateClinicianCommand ToCreateClinicianCommand(this CreateClinicianInputModel createClinicianInputModel)
    {
        return new CreateClinicianCommand(
            createClinicianInputModel.FirstName,
            createClinicianInputModel.LastName,
            createClinicianInputModel.ClinicianCategoryIds);
    }
    
    public static UpdateClinicianCommand ToUpdateClinicianCommand(
        this UpdateClinicianInputModel updateClinicianInputModel,
        Guid id)
    {
        return new UpdateClinicianCommand(
            id,
            updateClinicianInputModel.LastName,
            updateClinicianInputModel.ClinicianCategoryIds);
    }
}