using Application.Common.Exceptions;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Providers;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceCommand;

public class UpdateDeviceCommandHandler(
    IDeviceCategoryRepository deviceCategoryRepository,
    IDeviceRepository deviceRepository,
    IDateTimeProvider dateTimeProvider,
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
                throw new NotFoundException(nameof(Device), request.Id);
            
            // Querying and checking device categories
            var deviceCategories = await deviceCategoryRepository.GetByClinicIdAndDeviceCategoryIdsAsync(
                request.Clinic.Id,
                request.DeviceCategoryIds,
                cancellationToken);
            var missingDeviceCategoryIds = request.DeviceCategoryIds.Except(deviceCategories.Select(deviceCategory => deviceCategory.Id)).ToList();
            if (missingDeviceCategoryIds.Count > 0)
                throw new NotFoundException(nameof(DeviceCategory), missingDeviceCategoryIds);
            
            // Updating device
            device.Update(
                request.Name,
                request.Abbreviation,
                deviceCategories,
                request.DateOfLastMaintenance,
                dateTimeProvider.UtcNow);
            
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