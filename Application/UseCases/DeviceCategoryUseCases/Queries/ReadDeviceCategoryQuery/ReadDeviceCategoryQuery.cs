using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoryQuery;

public record ReadDeviceCategoryQuery(Guid Id) 
    : IRequest<DeviceCategoryDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}