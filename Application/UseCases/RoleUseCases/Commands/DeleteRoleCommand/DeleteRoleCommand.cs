using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoleUseCases.Commands.DeleteRoleCommand;

public record DeleteRoleCommand(Guid Id) : IRequest, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}