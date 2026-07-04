using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.CreateDeviceCategoryCommand;

public record CreateDeviceCategoryCommand(
    string Name,
    string Abbreviation,
    string Color,
    ICollection<Guid> DeviceIds)
    : IRequest<DeviceCategoryDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
} 