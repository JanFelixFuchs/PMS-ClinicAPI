using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.DeleteDeviceCategoryCommand;

public class DeleteDeviceCategoryCommandHandler(
    IDeviceCategoryRepository deviceCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteDeviceCategoryCommand>
{
    public async Task Handle(DeleteDeviceCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking device category
            var deviceCategory = await deviceCategoryRepository.GetByClinicIdAndDeviceCategoryIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                deviceCategory => deviceCategory.Devices);
            if (deviceCategory == null)
                throw new NotFoundException(nameof(DeviceCategory), request.Id);
            
            // Deleting device category
            deviceCategory.Delete(deviceCategory.Devices);
        }, cancellationToken);
    }
}