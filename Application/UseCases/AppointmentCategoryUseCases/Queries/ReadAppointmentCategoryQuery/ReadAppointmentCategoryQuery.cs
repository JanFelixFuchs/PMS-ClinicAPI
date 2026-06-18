using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoryQuery;

public record ReadAppointmentCategoryQuery(Guid Id) 
    : IRequest<AppointmentCategoryDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}