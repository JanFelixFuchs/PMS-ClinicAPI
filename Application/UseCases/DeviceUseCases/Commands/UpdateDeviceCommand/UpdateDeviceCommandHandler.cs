using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceCommand;

public class UpdateDeviceCommandHandler(
    ILogger<UpdateDeviceCommandHandler> logger,
    IDeviceCategoryRepository deviceCategoryRepository,
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateDeviceCommand, DeviceDetailedOutputModel>
{
    public async Task<DeviceDetailedOutputModel> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
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
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(device), request.Id);
                throw new NotFoundException(nameof(Device), request.Id);
            }
            
            // Querying and checking device categories
            var deviceCategories = await deviceCategoryRepository.GetByClinicIdAndDeviceCategoryIdsAsync(
                request.Clinic.Id,
                request.DeviceCategoryIds,
                cancellationToken);
            var missingDeviceCategoryIds = request.DeviceCategoryIds.Except(deviceCategories.Select(deviceCategory => deviceCategory.Id)).ToList();
            if (missingDeviceCategoryIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(deviceCategories), missingDeviceCategoryIds);
                throw new NotFoundException(nameof(DeviceCategory), missingDeviceCategoryIds);
            }
            
            // Updating device
            device.Update(
                request.Name,
                request.Abbreviation,
                deviceCategories,
                request.DateOfLastMaintenance);
            
            // Returning output model
            return new DeviceDetailedOutputModel(
                device,
                deviceCategories,
                device.Appointments,
                device.AppointmentProtocols,
                device.Results);
        }, cancellationToken);
    }
}