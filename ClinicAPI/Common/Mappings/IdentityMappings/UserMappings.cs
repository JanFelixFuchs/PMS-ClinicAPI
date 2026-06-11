using Application.UseCases.UserUseCases.Commands.CreateUserCommand;
using Application.UseCases.UserUseCases.Commands.UpdateUserCommand;
using PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

namespace PMS_ClinicAPI.Common.Mappings.IdentityMappings;

public static class UserMappings
{
    public static CreateUserCommand ToCreateUserCommand(this CreateUserInputModel createUserInputModel)
    {
        return new CreateUserCommand(
            createUserInputModel.Username,
            createUserInputModel.Password,
            createUserInputModel.RoleId!.Value,
            createUserInputModel.ClinicianId!.Value);
    }

    public static UpdateUserCommand ToUpdateUserCommand(
        this UpdateUserInputModel updateUserInputModel,
        Guid id)
    {
        return new UpdateUserCommand(
            id,
            updateUserInputModel.RoleId!.Value);
    }
}