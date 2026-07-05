using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentsQuery;

public record ReadAppointmentsQuery(
    DateTime StartDateTime,
    DateTime EndDateTime) 
    : IRequest<List<AppointmentOverviewOutputModel>>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}