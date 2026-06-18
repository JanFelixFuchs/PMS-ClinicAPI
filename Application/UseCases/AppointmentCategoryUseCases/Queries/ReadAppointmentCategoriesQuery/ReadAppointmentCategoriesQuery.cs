using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoriesQuery;

public record ReadAppointmentCategoriesQuery : IRequest<List<AppointmentCategoryOverviewOutputModel>>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}