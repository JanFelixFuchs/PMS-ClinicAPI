using Application.UseCases.RoleUseCases.Commands.CreateRoleCommand;
using Application.UseCases.RoleUseCases.Commands.UpdateRoleCommand;
using PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

namespace PMS_ClinicAPI.Common.Mappings.IdentityMappings;

public static class RoleMappings
{
    public static CreateRoleCommand ToCreateRoleCommand(this CreateRoleInputModel createRoleInputModel)
    {
        return new CreateRoleCommand(
            createRoleInputModel.Name,
            createRoleInputModel.Claims,
            createRoleInputModel.UserIds);
    }

    public static UpdateRoleCommand ToUpdateRoleCommand(
        this UpdateRoleInputModel updateRoleInputModel,
        Guid id)
    {
        return new UpdateRoleCommand(
            id,
            updateRoleInputModel.Name,
            updateRoleInputModel.Claims);
    }
}