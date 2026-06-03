using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.IdentityOutputModels;

public class UserOverviewOutputModel(User user)
{
    public Guid Id { get; init; } = user.Id;
    public string Username { get; init; } = user.Username;
    public bool IsAdmin { get; init; }= user.IsAdmin;
    public bool IsArchived { get; init; }= user.IsArchived;
}

public class UserDetailedOutputModel(
    User user, 
    Role role, 
    Clinician? clinician) 
    : UserOverviewOutputModel(user)
{
    public RoleOverviewOutputModel Role { get; init; } = new(role);
    public ClinicianOverviewOutputModel? Clinician { get; init; } = clinician != null ? new ClinicianOverviewOutputModel(clinician) : null;
}