using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AuthUseCases.Commands.UpdateClinicCodeCommand;

public class UpdateClinicCodeCommandHandler(
    ILogger<UpdateClinicCodeCommandHandler> logger,
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateClinicCodeCommand> 
{
    public Task Handle(UpdateClinicCodeCommand request, CancellationToken cancellationToken)
    {
        return unitOfWork.ExecuteAsync(async () =>
        {
            // Checking old code for correctness
            var normalizedOldCode = StringHelper.Normalize(request.OldCode);
            if (normalizedOldCode != request.Clinic.NormalizedCode)
            {
                logger.LogWarning(LogMessages.EntityPropertyInvalid, nameof(request.OldCode), nameof(Clinic));
                throw new InvalidPropertyValueException(nameof(Clinic), nameof(Clinic.Code));
            }
            
            // Checking codes for equality
            var normalizedNewCode = StringHelper.Normalize(request.NewCode);
            if (normalizedNewCode == normalizedOldCode)
            {
                logger.LogWarning(LogMessages.EntityPropertyUnchanged, nameof(request.NewCode), nameof(Clinic));
                throw new PropertyUnchangedException(nameof(Clinic), nameof(Clinic.Code));
            }
            
            // Checking new code for uniqueness
            var existingClinic = await clinicRepository.GetByNormalizedCodeAsync(normalizedNewCode, cancellationToken);
            if (existingClinic != null)
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.NewCode), nameof(Clinic));
                throw new PropertyAlreadyInUseException<string>(nameof(Clinic), nameof(Clinic.Code), request.NewCode);
            }
            
            // Updating clinic code
            request.Clinic.UpdateCode(request.NewCode);
        }, cancellationToken);
    }
}