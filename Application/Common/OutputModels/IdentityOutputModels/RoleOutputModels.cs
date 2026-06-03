using Application.Common.OutputModels.SharedOutputModels;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.IdentityOutputModels;

public class RoleOverviewOutputModel(Role role)
{
    public Guid Id { get; init; } = role.Id;
    public string Name { get; init; } = role.Name;
    public bool IsSystemRole { get; init; } = role.IsSystemRole;
}

public class RoleDetailedOutputModel(
    Role role, 
    ICollection<User> users,
    ICollection<Claim> claims) 
    : RoleOverviewOutputModel(role)
{
    public ICollection<UserOverviewOutputModel> Users { get; init; } = users.Select(user => new UserOverviewOutputModel(user)).ToList();
    public ICollection<ClaimOutputModel> Claims { get; init; } = claims.Select(claim => new ClaimOutputModel(claim)).ToList();   
}