using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Interfaces;
using Application.Common.OutputModels.DeviceOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Queries.ReadDevicesQuery;

public record ReadDevicesQuery(bool Archived) 
    : IRequest<List<DeviceOverviewOutputModel>>, IRequireRequestContext, IArchivableQuery
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}