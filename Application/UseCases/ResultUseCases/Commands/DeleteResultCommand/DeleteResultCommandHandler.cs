using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.DeviceEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ResultUseCases.Commands.DeleteResultCommand;

public class DeleteResultCommandHandler(
    ILogger<DeleteResultCommandHandler> logger,
    IResultRepository resultRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteResultCommand>
{
    public async Task Handle(DeleteResultCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking result
            var result = await resultRepository.GetByClinicIdAndResultIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken, 
                result => result.Patient);
            if (result == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(result), request.Id);
                throw new NotFoundException(nameof(Device), request.Id);
            }
            
            // Deleting result
            result.Delete(result.Patient);
        }, cancellationToken);
    }
}