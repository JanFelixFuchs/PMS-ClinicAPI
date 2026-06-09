using Application.UseCases.ResultUseCases.Commands.CreateResultCommand;
using PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

namespace PMS_ClinicAPI.Common.Mappings.AppointmentMappings;

public static class ResultMappings
{
    public static CreateResultCommand ToCreateResultCommand(this CreateResultInputModel createResultInputModel)
    {
        return new CreateResultCommand(
            createResultInputModel.Title,
            createResultInputModel.DateOfCreation!.Value,
            createResultInputModel.Appendix,
            createResultInputModel.Remarks,
            createResultInputModel.PatientId!.Value,
            createResultInputModel.ClinicianId!.Value,
            createResultInputModel.DeviceId);
    }
}