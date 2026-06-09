using Application.UseCases.AppointmentUseCases.Commands.CreateAppointmentCommand;
using Application.UseCases.AppointmentUseCases.Commands.UpdateAppointmentCommand;
using PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

namespace PMS_ClinicAPI.Common.Mappings.AppointmentMappings;

public static  class AppointmentMappings
{
    public static CreateAppointmentCommand ToCreateAppointmentCommand(this CreateAppointmentInputModel createAppointmentInputModel)
    {
        return new CreateAppointmentCommand(
            createAppointmentInputModel.Title,
            createAppointmentInputModel.StartTime!.Value,
            createAppointmentInputModel.EndTime!.Value,
            createAppointmentInputModel.AppointmentCategoryIds,
            createAppointmentInputModel.PatientId!.Value,
            createAppointmentInputModel.RoomId!.Value,
            createAppointmentInputModel.DeviceIds,
            createAppointmentInputModel.ClinicianIds);
    }
    
    public static UpdateAppointmentCommand ToUpdateAppointmentCommand(
        this UpdateAppointmentInputModel updateAppointmentInputModel,
        Guid id)
    {
        return new UpdateAppointmentCommand(
            id,
            updateAppointmentInputModel.Title,
            updateAppointmentInputModel.StartTime!.Value,
            updateAppointmentInputModel.EndTime!.Value,
            updateAppointmentInputModel.AppointmentCategoryIds,
            updateAppointmentInputModel.PatientId!.Value,
            updateAppointmentInputModel.RoomId!.Value,
            updateAppointmentInputModel.DeviceIds,
            updateAppointmentInputModel.ClinicianIds);
    }
}