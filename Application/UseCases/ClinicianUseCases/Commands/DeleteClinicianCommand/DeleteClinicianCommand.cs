using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Commands.DeleteClinicianCommand;

public record DeleteClinicianCommand(Guid Id) : IRequest, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}