using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceStatusCommand;

public record UpdateDeviceStatusCommand(
    Guid Id,
    DeviceStatus Status)
    : IRequest<DeviceDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}