using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

public record CreateUserInputModel(
    [Required] string Username,
    [Required] string Password,
    [Required] Guid? RoleId,
    [Required] Guid? ClinicianId);

public record UpdateUserInputModel(
    [Required] Guid? RoleId);