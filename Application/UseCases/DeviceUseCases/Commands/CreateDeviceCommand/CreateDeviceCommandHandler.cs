using Application.Common.Exceptions;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Providers;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Commands.CreateDeviceCommand;

public class CreateDeviceCommandHandler(
    IDeviceCategoryRepository deviceCategoryRepository,
    IDeviceRepository deviceRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)  
    : IRequestHandler<CreateDeviceCommand, DeviceDetailedOutputModel>
{
    public async Task<DeviceDetailedOutputModel> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking device categories
            var deviceCategories = await deviceCategoryRepository.GetByClinicIdAndDeviceCategoryIdsAsync(
                request.Clinic.Id, 
                request.DeviceCategoryIds, 
                cancellationToken);
            var missingDeviceCategoryIds = request.DeviceCategoryIds.Except(deviceCategories.Select(deviceCategory => deviceCategory.Id)).ToList();
            if (missingDeviceCategoryIds.Count > 0)
                throw new NotFoundException(nameof(DeviceCategory), missingDeviceCategoryIds);
            
            // Creating device
            var device = new Device(
                request.Clinic,
                request.Name,
                request.Abbreviation,
                request.SerialNumber,
                request.Status,
                request.Producer,
                deviceCategories,
                request.DateOfPurchase,
                request.DateOfLastMaintenance,
                dateTimeProvider.UtcNow);

            // Adding device
            await deviceRepository.AddAsync(device, cancellationToken);
            
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