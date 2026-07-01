using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Transactions;
using MediatR;

namespace Application.UseCases.ClinicUseCases.Commands.UpdateClinicCommand;

public class UpdateClinicCommandHandler(
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateClinicCommand, ClinicOutputModel>
{
    public Task<ClinicOutputModel> Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
    {
        return unitOfWork.ExecuteAsync(() =>
        {
            // Updating clinic
            request.Clinic.Update(
                request.Name,
                request.Abbreviation, 
                request.Owner,
                request.MedicalField,
                request.Street,
                request.HouseNumber,
                request.City,
                request.ZipCode,
                request.Country,
                request.Email,
                request.PhoneNumber);
            
            // Returning output model
            return Task.FromResult(new ClinicOutputModel(request.Clinic));
        }, cancellationToken);
    }
}
