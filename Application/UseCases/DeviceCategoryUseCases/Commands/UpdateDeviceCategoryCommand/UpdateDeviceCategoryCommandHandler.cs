using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.UpdateDeviceCategoryCommand;

public class UpdateDeviceCategoryCommandHandler(
    ILogger<UpdateDeviceCategoryCommandHandler> logger,
    IDeviceRepository deviceRepository,
    IDeviceCategoryRepository deviceCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateDeviceCategoryCommand, DeviceCategoryDetailedOutputModel>
{
    public async Task<DeviceCategoryDetailedOutputModel> Handle(UpdateDeviceCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
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
            
            // Updating device category
            deviceCategory.Update(request.Name, request.Abbreviation, request.Color);
            
            // Defining current devices
            var currentDevices = deviceCategory.Devices.ToList();
            var currentDeviceIds = currentDevices.Select(device => device.Id).ToHashSet();
            
            // Calculating unchanged devices
            var unchangedDevices = currentDevices.Where(device => request.DeviceIds.Contains(device.Id)).ToList();
            
            // Calculating devices to be changed
            var deviceIdsToRemoveFrom = currentDevices.Select(device => device.Id).Except(request.DeviceIds).ToHashSet();
            var deviceIdsToAddTo = request.DeviceIds.Except(currentDeviceIds).ToHashSet();
            
            // Removing device categories from non-assigned devices
            var devicesToRemoveFrom = currentDevices.Where(device => deviceIdsToRemoveFrom.Contains(device.Id)).ToList();
            foreach (var device in devicesToRemoveFrom)
                device.RemoveDeviceCategory(deviceCategory);

            // Adding device categories to newly assigned devices
            ICollection<Device> devicesToAddTo = new List<Device>();
            if (deviceIdsToAddTo.Count > 0)
            {
                devicesToAddTo = await deviceRepository.GetByClinicIdAndDeviceIdsAsync(
                    request.Clinic.Id,
                    deviceIdsToAddTo,
                    cancellationToken);
                var missingDeviceIds = deviceIdsToAddTo.Except(devicesToAddTo.Select(device => device.Id)).ToList();
                if (missingDeviceIds.Count > 0)
                {
                    logger.LogWarning(LogMessages.EntitiesNotFound, nameof(devicesToAddTo), missingDeviceIds);
                    throw new NotFoundException(nameof(Device), missingDeviceIds);
                }
        
                foreach (var device in devicesToAddTo)
                    device.AddDeviceCategory(deviceCategory);
            }
            
            // Returning output model
            return new DeviceCategoryDetailedOutputModel(deviceCategory, unchangedDevices.Concat(devicesToAddTo).ToList());
        }, cancellationToken);
    }
}