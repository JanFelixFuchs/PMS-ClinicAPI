using Application.UseCases.AppointmentCategoryUseCases.Commands.CreateAppointmentCategoryCommand;
using Application.UseCases.AppointmentCategoryUseCases.Commands.UpdateAppointmentCategoryCommand;
using PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

namespace PMS_ClinicAPI.Common.Mappings.AppointmentMappings;

public static class AppointmentCategoryMappings
{
    public static CreateAppointmentCategoryCommand ToCreateAppointmentCategoryCommand(this CreateAppointmentCategoryInputModel createAppointmentCategoryInputModel)
    {
        return new CreateAppointmentCategoryCommand(
            createAppointmentCategoryInputModel.Name,
            createAppointmentCategoryInputModel.Abbreviation, 
            createAppointmentCategoryInputModel.Color);
    }
    
    public static UpdateAppointmentCategoryCommand ToUpdateAppointmentCategoryCommand(
        this UpdateAppointmentCategoryInputModel updateAppointmentCategoryInputModel, 
        Guid id)
    {
        return new UpdateAppointmentCategoryCommand(
            id,
            updateAppointmentCategoryInputModel.Name,
            updateAppointmentCategoryInputModel.Abbreviation,
            updateAppointmentCategoryInputModel.Color);
    }
}