using Application.Common.Exceptions;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceStatusCommand;

public class UpdateDeviceStatusCommandHandler(
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateDeviceStatusCommand, DeviceDetailedOutputModel>
{
    public async Task<DeviceDetailedOutputModel> Handle(UpdateDeviceStatusCommand request, CancellationToken cancellationToken)
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

            // Updating device status
            device.ChangeStatus(request.Status, device.Appointments);

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