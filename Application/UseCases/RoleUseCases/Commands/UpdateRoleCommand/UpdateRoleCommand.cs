using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoleUseCases.Commands.UpdateRoleCommand;

public record UpdateRoleCommand( 
    Guid Id,
    string Name,
    Dictionary<ClaimType, ClaimValue> Claims)
    : IRequest<RoleDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}