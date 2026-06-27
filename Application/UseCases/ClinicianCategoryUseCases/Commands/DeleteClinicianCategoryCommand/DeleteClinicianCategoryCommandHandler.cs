using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ClinicianCategoryUseCases.Commands.DeleteClinicianCategoryCommand;

public class DeleteClinicianCategoryCommandHandler(
    ILogger<DeleteClinicianCategoryCommandHandler> logger,
    IClinicianCategoryRepository clinicianCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteClinicianCategoryCommand>
{
    public async Task Handle(DeleteClinicianCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking clinician category
            var clinicianCategory = await clinicianCategoryRepository.GetByClinicIdAndClinicianCategoryIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                clinicianCategory => clinicianCategory.Clinicians);
            if (clinicianCategory == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(clinicianCategory), request.Id);
                throw new NotFoundException(nameof(ClinicianCategory), request.Id);
            }
            
            // Deleting clinician category
            clinicianCategory.Delete(clinicianCategory.Clinicians);
        }, cancellationToken);
    }
}