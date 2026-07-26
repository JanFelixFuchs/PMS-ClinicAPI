using Application.Common.Exceptions;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Queries.ReadDeviceQuery;

public class ReadDeviceQueryHandler(IDeviceRepository deviceRepository)
    : IRequestHandler<ReadDeviceQuery, DeviceDetailedOutputModel>
{
    public async Task<DeviceDetailedOutputModel> Handle(ReadDeviceQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking device
        var device = await deviceRepository.GetByClinicIdAndDeviceIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken,
            device => device.DeviceCategories,
            device => device.Appointments,
            device => device.AppointmentProtocols,
            device => device.Results);
        if (device == null)
            throw new NotFoundException(nameof(Device), request.Id);
        
        // Returning output model
        return new DeviceDetailedOutputModel(
            device,
            device.DeviceCategories,
            device.Appointments,
            device.AppointmentProtocols,
            device.Results);
    }
}