using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentProtocolUseCases.Queries.ReadAppointmentProtocolQuery;

public record ReadAppointmentProtocolQuery(Guid Id) 
    : IRequest<AppointmentProtocolDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}