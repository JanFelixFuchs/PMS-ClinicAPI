using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoleUseCases.Commands.CreateRoleCommand;

public record CreateRoleCommand(
    string Name,
    Dictionary<ClaimType, ClaimValue> Claims,
    ICollection<Guid> UserIds) 
    : IRequest<RoleDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}