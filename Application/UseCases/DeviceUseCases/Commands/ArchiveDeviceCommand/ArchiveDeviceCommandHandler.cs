using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeviceUseCases.Commands.ArchiveDeviceCommand;

public class ArchiveDeviceCommandHandler(
    ILogger<ArchiveDeviceCommandHandler> logger,
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ArchiveDeviceCommand, DeviceDetailedOutputModel>
{
    public async Task<DeviceDetailedOutputModel> Handle(ArchiveDeviceCommand request, CancellationToken cancellationToken)
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

            // Archiving device
            device.Archive(device.Appointments, device.AppointmentProtocols);
            
            // Returning output model
            return new DeviceDetailedOutputModel(
                device,
                device.DeviceCategories,
                device.Appointments,
                device.AppointmentProtocols,
                device.Results);
        }, cancellationToken);
    }
}