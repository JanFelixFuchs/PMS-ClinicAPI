using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Interfaces;
using Application.Common.OutputModels.PatientOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Queries.ReadPatientsQuery;

public record ReadPatientsQuery(bool Archived) 
    : IRequest<List<PatientOverviewOutputModel>>, IRequireRequestContext, IArchivableQuery
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}