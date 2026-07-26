using Application.Common.Exceptions;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;

namespace Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoryQuery;

public class ReadDeviceCategoryQueryHandler(IDeviceCategoryRepository deviceCategoryRepository) 
    : IRequestHandler<ReadDeviceCategoryQuery, DeviceCategoryDetailedOutputModel>
{
    public async Task<DeviceCategoryDetailedOutputModel> Handle(ReadDeviceCategoryQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking device category
        var deviceCategory = await deviceCategoryRepository.GetByClinicIdAndDeviceCategoryIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken, 
            deviceCategory => deviceCategory.Devices);
        if (deviceCategory == null)
            throw new NotFoundException(nameof(DeviceCategory), request.Id);
        
        // Returning output model
        return new DeviceCategoryDetailedOutputModel(deviceCategory, deviceCategory.Devices);
    }
}