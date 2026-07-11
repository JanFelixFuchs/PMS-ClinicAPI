using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoryQuery;

public class ReadDeviceCategoryQueryHandler(
    ILogger<ReadDeviceCategoryQueryHandler> logger,
    IDeviceCategoryRepository deviceCategoryRepository) 
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
        {
            logger.LogWarning(LogMessages.EntityNotFound, nameof(deviceCategory), request.Id);
            throw new NotFoundException(nameof(DeviceCategory), request.Id);
        }
        
        // Returning output model
        return new DeviceCategoryDetailedOutputModel(deviceCategory, deviceCategory.Devices);
    }
}