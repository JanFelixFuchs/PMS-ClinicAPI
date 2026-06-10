using Application.UseCases.PatientUseCases.Commands.CreatePatientCommand;
using Application.UseCases.PatientUseCases.Commands.UpdatePatientCommand;
using PMS_ClinicAPI.Common.InputModels.PatientInputModels;

namespace PMS_ClinicAPI.Common.Mappings.PatientMappings;

public static class PatientMappings
{
    public static CreatePatientCommand ToCreatePatientCommand(this CreatePatientInputModel createPatientInputModel)
    {
        return new CreatePatientCommand(
            createPatientInputModel.FirstName,
            createPatientInputModel.LastName,
            createPatientInputModel.DateOfBirth!.Value,
            createPatientInputModel.Gender!.Value,
            createPatientInputModel.Street,
            createPatientInputModel.HouseNumber,
            createPatientInputModel.City,
            createPatientInputModel.ZipCode,
            createPatientInputModel.Country!.Value,
            createPatientInputModel.Email,
            createPatientInputModel.PhoneNumber,
            createPatientInputModel.InsuranceStatus!.Value,
            createPatientInputModel.Allergies,
            createPatientInputModel.Remarks);
    }

    public static UpdatePatientCommand ToUpdatePatientCommand(
        this UpdatePatientInputModel updatePatientInputModel,
        Guid id)
    {
        return new UpdatePatientCommand(
            id,
            updatePatientInputModel.LastName,
            updatePatientInputModel.Street,
            updatePatientInputModel.HouseNumber,
            updatePatientInputModel.City,
            updatePatientInputModel.ZipCode,
            updatePatientInputModel.Country!.Value,
            updatePatientInputModel.Email,
            updatePatientInputModel.PhoneNumber,
            updatePatientInputModel.InsuranceStatus!.Value,
            updatePatientInputModel.Allergies,
            updatePatientInputModel.Remarks);
    }
}