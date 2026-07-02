using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Commands.CreateClinicianCommand;

public record CreateClinicianCommand(
    string FirstName,
    string LastName,
    ICollection<Guid> ClinicianCategoryIds) 
    : IRequest<ClinicianDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}