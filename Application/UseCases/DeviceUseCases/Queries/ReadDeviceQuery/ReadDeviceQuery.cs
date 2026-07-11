using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Queries.ReadDeviceQuery;

public record ReadDeviceQuery(Guid Id) : IRequest<DeviceDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}