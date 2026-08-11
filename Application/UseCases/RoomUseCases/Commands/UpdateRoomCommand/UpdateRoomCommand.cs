using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.RoomOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Commands.UpdateRoomCommand;

public record UpdateRoomCommand(
    Guid Id,
    string Name,
    string Abbreviation,
    ICollection<Guid> RoomCategoryIds,
    string? RoomNumber,
    string? Floor,
    string? Building)
    : IRequest<RoomDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}