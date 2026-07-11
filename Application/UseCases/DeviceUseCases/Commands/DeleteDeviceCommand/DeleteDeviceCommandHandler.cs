using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeviceUseCases.Commands.DeleteDeviceCommand;

public class DeleteDeviceCommandHandler(
    ILogger<DeleteDeviceCommandHandler> logger,
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDeviceCommand>
{
    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking device
            var device = await deviceRepository.GetByClinicIdAndDeviceIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                device => device.Appointments,
                device => device.AppointmentProtocols,
                device => device.Results);
            if (device == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(device), request.Id);
                throw new NotFoundException(nameof(Device), request.Id);
            }
            
            // Deleting device
            device.Delete(device.Appointments, device.AppointmentProtocols, device.Results);
        }, cancellationToken);
    }
}