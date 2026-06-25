using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.UpdatePasswordCommand;

public record UpdatePasswordCommand(
    string OldPassword,
    string NewPassword)
    : IRequest<(UpdatePasswordOutputModel Payload, string RefreshToken)>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}