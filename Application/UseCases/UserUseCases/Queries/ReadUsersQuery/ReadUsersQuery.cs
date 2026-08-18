using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Interfaces;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Queries.ReadUsersQuery;

public record ReadUsersQuery(bool Archived)
    : IRequest<List<UserOverviewOutputModel>>, IRequireRequestContext, IArchivableQuery
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}