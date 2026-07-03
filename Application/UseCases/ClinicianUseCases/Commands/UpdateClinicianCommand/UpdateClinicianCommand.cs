using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Commands.UpdateClinicianCommand;

public record UpdateClinicianCommand(
    Guid Id,
    string LastName,
    ICollection<Guid> ClinicianCategoryIds)
    : IRequest<ClinicianDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}