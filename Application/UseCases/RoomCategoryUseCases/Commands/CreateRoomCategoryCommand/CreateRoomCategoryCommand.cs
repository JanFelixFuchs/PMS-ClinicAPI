using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.RoomOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoomCategoryUseCases.Commands.CreateRoomCategoryCommand;

public record CreateRoomCategoryCommand(
    string Name,
    string Abbreviation,
    string Color,
    ICollection<Guid> RoomIds)
    : IRequest<RoomCategoryDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
} 