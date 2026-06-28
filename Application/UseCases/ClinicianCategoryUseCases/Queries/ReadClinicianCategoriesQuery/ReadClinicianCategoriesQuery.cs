using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicianCategoryUseCases.Queries.ReadClinicianCategoriesQuery;

public record ReadClinicianCategoriesQuery : IRequest<List<ClinicianCategoryOverviewOutputModel>>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}