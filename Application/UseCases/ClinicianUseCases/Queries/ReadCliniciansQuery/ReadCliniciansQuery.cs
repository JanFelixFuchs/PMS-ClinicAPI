using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Interfaces;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Queries.ReadCliniciansQuery;

public record ReadCliniciansQuery(bool Archived)
    : IRequest<List<ClinicianOverviewOutputModel>>, IRequireRequestContext, IArchivableQuery
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}