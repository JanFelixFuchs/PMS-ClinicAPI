using Application.UseCases.ClinicUseCases.Commands.UpdateClinicCommand;
using PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

namespace PMS_ClinicAPI.Common.Mappings.IdentityMappings;

public static class ClinicMappings
{
    public static UpdateClinicCommand ToUpdateClinicCommand(this UpdateClinicInputModel updateClinicInputModel)
    {
        return new UpdateClinicCommand(
            updateClinicInputModel.Name,
            updateClinicInputModel.Abbreviation,
            updateClinicInputModel.Owner,
            updateClinicInputModel.MedicalField!.Value,
            updateClinicInputModel.Street,
            updateClinicInputModel.HouseNumber,
            updateClinicInputModel.City,
            updateClinicInputModel.ZipCode,
            updateClinicInputModel.Country!.Value,
            updateClinicInputModel.Email,
            updateClinicInputModel.PhoneNumber);
    }
}