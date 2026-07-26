using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.UpdateClinicCodeCommand;

public class UpdateClinicCodeCommandHandler(
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
                throw new IncorrectPropertyValueException(nameof(Clinic), nameof(Clinic.Code));
            
            // Checking codes for equality
            var normalizedNewCode = StringHelper.Normalize(request.NewCode);
            if (normalizedNewCode == normalizedOldCode)
                throw new UnchangedPropertyValueException(nameof(Clinic), nameof(Clinic.Code));
            
            // Checking new code for uniqueness
            var existingClinic = await clinicRepository.GetByNormalizedCodeAsync(normalizedNewCode, cancellationToken);
            if (existingClinic != null)
                throw new PropertyValueAlreadyInUseException<string>(nameof(Clinic), nameof(Clinic.Code), request.NewCode);
            
            // Updating clinic code
            request.Clinic.UpdateCode(request.NewCode);
        }, cancellationToken);
    }
}