using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.SharedOutputModels;

public class CurrentUserOutputModel(User user, Role role, Clinician? clinician)
{
    public string Username { get; init; } = user.Username;
    public bool IsAdmin { get; init; } = user.IsAdmin;
    public string RoleName { get; init; } = role.Name;
    public string? FirstName { get; init; } = clinician?.FirstName;
    public string? LastName { get; init; } = clinician?.LastName;
}