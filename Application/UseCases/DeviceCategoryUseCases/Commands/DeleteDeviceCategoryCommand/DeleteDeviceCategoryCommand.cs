using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.DeleteDeviceCategoryCommand;

public record DeleteDeviceCategoryCommand(Guid Id) : IRequest, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}