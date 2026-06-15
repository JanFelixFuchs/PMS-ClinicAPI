using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;

namespace Application.UseCases.AppointmentCategoryUseCases.Commands.CreateAppointmentCategoryCommand;

public class CreateAppointmentCategoryCommandHandler(
    IAppointmentCategoryRepository appointmentCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateAppointmentCategoryCommand, AppointmentCategoryDetailedOutputModel>
{
    public async Task<AppointmentCategoryDetailedOutputModel> Handle(CreateAppointmentCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Creating appointment category
            var appointmentCategory = new AppointmentCategory(
                request.Clinic, 
                request.Name, 
                request.Abbreviation, 
                request.Color);
            
            // Adding appointment category
            await appointmentCategoryRepository.AddAsync(appointmentCategory, cancellationToken);
            
            // Returning output model
            return new AppointmentCategoryDetailedOutputModel(appointmentCategory, appointmentCategory.Appointments);
        }, cancellationToken);
    }
}