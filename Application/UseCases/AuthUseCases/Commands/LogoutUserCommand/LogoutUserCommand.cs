using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.LogoutUserCommand;

public record LogoutUserCommand : IRequest, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}