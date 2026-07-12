using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceCommand;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    string Abbreviation,
    ICollection<Guid> DeviceCategoryIds,
    DateTime? DateOfLastMaintenance)
    : IRequest<DeviceDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}