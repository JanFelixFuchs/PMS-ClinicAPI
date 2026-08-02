using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Interfaces;
using Application.Common.OutputModels.RoomOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Queries.ReadRoomsQuery;

public record ReadRoomsQuery(bool Archived) 
    : IRequest<List<RoomOverviewOutputModel>>, IRequireRequestContext, IArchivableQuery
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}