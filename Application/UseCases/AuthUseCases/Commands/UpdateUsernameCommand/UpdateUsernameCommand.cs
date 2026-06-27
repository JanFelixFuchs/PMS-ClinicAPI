using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.UpdateUsernameCommand;

public record UpdateUsernameCommand(
    string OldUsername,
    string NewUsername)
    : IRequest<UpdateUsernameOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}