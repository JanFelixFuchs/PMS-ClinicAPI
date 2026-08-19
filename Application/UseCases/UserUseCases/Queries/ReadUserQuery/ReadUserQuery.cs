using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Queries.ReadUserQuery;

public record ReadUserQuery(Guid Id) : IRequest<UserDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}