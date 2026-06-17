using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentCategoryUseCases.Commands.UpdateAppointmentCategoryCommand;

public class UpdateAppointmentCategoryCommandHandler(
    ILogger<UpdateAppointmentCategoryCommandHandler> logger,
    IAppointmentCategoryRepository appointmentCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateAppointmentCategoryCommand, AppointmentCategoryDetailedOutputModel>
{
    public async Task<AppointmentCategoryDetailedOutputModel> Handle(UpdateAppointmentCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking appointment category
            var appointmentCategory = await appointmentCategoryRepository.GetByClinicIdAndAppointmentCategoryIdAsync(
                request.Clinic.Id,
                request.Id,
                cancellationToken,
                appointmentCategory => appointmentCategory.Appointments);
            if (appointmentCategory == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(appointmentCategory), request.Id);
                throw new NotFoundException(nameof(AppointmentCategory), request.Id);
            }
            
            // Updating appointment category
            appointmentCategory.Update(request.Name, request.Abbreviation, request.Color);
            
            // Returning output model
            return new AppointmentCategoryDetailedOutputModel(appointmentCategory, appointmentCategory.Appointments);
        }, cancellationToken);
    }
}