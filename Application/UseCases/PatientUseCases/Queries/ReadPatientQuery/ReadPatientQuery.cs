using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.PatientOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Queries.ReadPatientQuery;

public record ReadPatientQuery(Guid Id) : IRequest<PatientDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}