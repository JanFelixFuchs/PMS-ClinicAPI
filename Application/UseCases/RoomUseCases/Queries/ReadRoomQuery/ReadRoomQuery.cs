using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.RoomOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Queries.ReadRoomQuery;

public record ReadRoomQuery(Guid Id) : IRequest<RoomDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}