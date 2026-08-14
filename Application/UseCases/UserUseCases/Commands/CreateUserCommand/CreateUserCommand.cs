using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.CreateUserCommand;

public record CreateUserCommand(
    string Username,
    string Password,
    Guid RoleId,
    Guid ClinicianId) 
    : IRequest<UserDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}