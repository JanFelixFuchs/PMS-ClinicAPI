using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianCategoryUseCases.Commands.UpdateClinicianCategoryCommand;

public record UpdateClinicianCategoryCommand(
    Guid Id,
    string Name,
    string Abbreviation,
    string Color,
    ICollection<Guid> ClinicianIds) 
    : IRequest<ClinicianCategoryDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}