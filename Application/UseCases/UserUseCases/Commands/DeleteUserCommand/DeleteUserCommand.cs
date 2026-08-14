using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.DeleteUserCommand;

public record DeleteUserCommand(Guid Id) : IRequest, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}