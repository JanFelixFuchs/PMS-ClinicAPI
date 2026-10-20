using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;

namespace Application.UseCases.ResultUseCases.Commands.DeleteResultCommand;

public class DeleteResultCommandHandler(
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
                throw new NotFoundException(nameof(Result), request.Id);
            
            // Deleting result
            result.Delete(result.Patient);
        }, cancellationToken);
    }
}