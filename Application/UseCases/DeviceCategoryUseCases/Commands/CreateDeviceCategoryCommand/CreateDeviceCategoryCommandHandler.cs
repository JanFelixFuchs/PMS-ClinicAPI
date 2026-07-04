using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.CreateDeviceCategoryCommand;

public class CreateDeviceCategoryCommandHandler(
    ILogger<CreateDeviceCategoryCommandHandler> logger,
    IDeviceRepository deviceRepository,
    IDeviceCategoryRepository deviceCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateDeviceCategoryCommand, DeviceCategoryDetailedOutputModel>
{
    public async Task<DeviceCategoryDetailedOutputModel> Handle(CreateDeviceCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Creating device category
            var deviceCategory = new DeviceCategory(request.Clinic, request.Name, request.Abbreviation, request.Color);
            
            // Adding device category
            await deviceCategoryRepository.AddAsync(deviceCategory, cancellationToken);
            
            // Querying and checking devices
            var devices = await deviceRepository.GetByClinicIdAndDeviceIdsAsync(
                request.Clinic.Id, 
                request.DeviceIds, 
                cancellationToken);
            var missingDeviceIds = request.DeviceIds.Except(devices.Select(device => device.Id)).ToList();
            if (missingDeviceIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(devices), missingDeviceIds);
                throw new NotFoundException(nameof(Device), missingDeviceIds);
            }
            
            // Adding device category to devices
            foreach (var device in devices)
                device.AddDeviceCategory(deviceCategory);
            
            // Returning output model
            return new DeviceCategoryDetailedOutputModel(deviceCategory, devices);
        }, cancellationToken);
    }
}