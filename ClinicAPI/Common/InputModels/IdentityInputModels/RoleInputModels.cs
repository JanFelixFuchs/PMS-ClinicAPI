using System.ComponentModel.DataAnnotations;
using Domain.Commons.Enums;

namespace PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

public record CreateRoleInputModel(
    [Required] string Name,
    [Required] Dictionary<ClaimType, ClaimValue> Claims,
    [Required] ICollection<Guid> UserIds);

public record UpdateRoleInputModel(
    [Required] string Name,
    [Required] Dictionary<ClaimType, ClaimValue> Claims);