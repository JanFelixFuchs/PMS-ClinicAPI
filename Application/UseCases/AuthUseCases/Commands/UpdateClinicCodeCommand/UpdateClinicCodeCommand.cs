using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.UpdateClinicCodeCommand;

public record UpdateClinicCodeCommand(
    string OldCode,
    string NewCode) 
    : IRequest, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}