using Application.UseCases.AuthUseCases.Commands.LoginUserCommand;
using Application.UseCases.AuthUseCases.Commands.RegisterClinicCommand;
using Application.UseCases.AuthUseCases.Commands.UpdateClinicCodeCommand;
using Application.UseCases.AuthUseCases.Commands.UpdatePasswordCommand;
using Application.UseCases.AuthUseCases.Commands.UpdateUsernameCommand;
using PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

namespace PMS_ClinicAPI.Common.Mappings.IdentityMappings;

public static class AuthMappings
{
    public static RegisterClinicCommand ToRegisterClinicCommand(this RegisterClinicInputModel registerClinicInputModel)
    {
        return new RegisterClinicCommand(
            registerClinicInputModel.Code,
            registerClinicInputModel.Name,
            registerClinicInputModel.Abbreviation,
            registerClinicInputModel.Owner,
            registerClinicInputModel.MedicalField!.Value,
            registerClinicInputModel.Street,
            registerClinicInputModel.HouseNumber,
            registerClinicInputModel.City,
            registerClinicInputModel.ZipCode,
            registerClinicInputModel.Country!.Value,
            registerClinicInputModel.Email,
            registerClinicInputModel.PhoneNumber,
            registerClinicInputModel.Username,
            registerClinicInputModel.Password,
            registerClinicInputModel.RoleNameWithNoRights,
            registerClinicInputModel.RoleNameWithAllRights);
    }
    
    public static LoginUserCommand ToLoginUserCommand(this LoginUserInputModel loginUserInputModel)
    {
        return new LoginUserCommand(
            loginUserInputModel.Code,
            loginUserInputModel.Username,
            loginUserInputModel.Password);
    }
    
    public static UpdateClinicCodeCommand ToUpdateClinicCodeCommand(this UpdateClinicCodeInputModel updateClinicCodeInputModel)
    {
        return new UpdateClinicCodeCommand(
            updateClinicCodeInputModel.OldCode,
            updateClinicCodeInputModel.NewCode);
    }

    public static UpdateUsernameCommand ToUpdateUsernameCommand(this UpdateUsernameInputModel updateUsernameInputModel)
    {
        return new UpdateUsernameCommand(
            updateUsernameInputModel.OldUsername,
            updateUsernameInputModel.NewUsername);
    }

    public static UpdatePasswordCommand ToUpdatePasswordCommand(this UpdatePasswordInputModel updatePasswordInputModel)
    {
        return new UpdatePasswordCommand(
            updatePasswordInputModel.OldPassword,
            updatePasswordInputModel.NewPassword);
    }
}