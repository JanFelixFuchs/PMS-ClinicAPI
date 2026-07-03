using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Queries.ReadClinicianQuery;

public record ReadClinicianQuery(Guid Id) : IRequest<ClinicianDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}