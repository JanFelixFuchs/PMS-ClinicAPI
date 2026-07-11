using Application.Common.OutputModels.DeviceOutputModels;
using Application.Repositories.DeviceRepositories;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Queries.ReadDevicesQuery;

public class ReadDevicesQueryHandler(IDeviceRepository deviceRepository)
    : IRequestHandler<ReadDevicesQuery, List<DeviceOverviewOutputModel>>
{
    public async Task<List<DeviceOverviewOutputModel>> Handle(ReadDevicesQuery request, CancellationToken cancellationToken)
    {
        // Querying devices
        var devices = await deviceRepository.GetByClinicIdAsync(
            request.Clinic.Id,
            request.Archived,
            cancellationToken);
        
        // Returning output model
        return devices
            .Select(device => new DeviceOverviewOutputModel(device))
            .ToList();
    }
}