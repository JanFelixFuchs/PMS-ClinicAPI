using Application.UseCases.ClinicianCategoryUseCases.Commands.CreateClinicianCategoryCommand;
using Application.UseCases.ClinicianCategoryUseCases.Commands.UpdateClinicianCategoryCommand;
using PMS_ClinicAPI.Common.InputModels.ClinicianInputModels;

namespace PMS_ClinicAPI.Common.Mappings.ClinicianMappings;

public static class ClinicianCategoryMappings
{
    public static CreateClinicianCategoryCommand ToCreateClinicianCategoryCommand(this CreateClinicianCategoryInputModel createClinicianCategoryInputModel)
    {
        return new CreateClinicianCategoryCommand(
            createClinicianCategoryInputModel.Name,
            createClinicianCategoryInputModel.Abbreviation, 
            createClinicianCategoryInputModel.Color,
            createClinicianCategoryInputModel.ClinicianIds);
    }
    
    public static UpdateClinicianCategoryCommand ToUpdateClinicianCategoryCommand(
        this UpdateClinicianCategoryInputModel updateClinicianCategoryInputModel,
        Guid id)
    {
        return new UpdateClinicianCategoryCommand(
            id,
            updateClinicianCategoryInputModel.Name,
            updateClinicianCategoryInputModel.Abbreviation,
            updateClinicianCategoryInputModel.Color,
            updateClinicianCategoryInputModel.ClinicianIds);
    }
}