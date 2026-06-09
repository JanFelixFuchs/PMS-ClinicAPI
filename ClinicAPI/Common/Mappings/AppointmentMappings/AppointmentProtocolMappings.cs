using Application.UseCases.AppointmentProtocolUseCases.Commands.UpdateAppointmentProtocolCommand;
using PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

namespace PMS_ClinicAPI.Common.Mappings.AppointmentMappings;

public static class AppointmentProtocolMappings
{
    public static UpdateAppointmentProtocolCommand ToUpdateAppointmentProtocolCommand(
        this UpdateAppointmentProtocolInputModel updateAppointmentProtocolInputModel,
        Guid id)
    {
        return new UpdateAppointmentProtocolCommand(
            id,
            updateAppointmentProtocolInputModel.Symptoms,
            updateAppointmentProtocolInputModel.Diagnosis,
            updateAppointmentProtocolInputModel.Treatment,
            updateAppointmentProtocolInputModel.Remarks,
            updateAppointmentProtocolInputModel.ClinicianId!.Value,
            updateAppointmentProtocolInputModel.RoomId!.Value,
            updateAppointmentProtocolInputModel.DeviceIds);
    }
}