using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AppointmentCategoryUseCases.Commands.DeleteAppointmentCategoryCommand;

public class DeleteAppointmentCategoryCommandHandler(
    ILogger<DeleteAppointmentCategoryCommandHandler> logger,
    IAppointmentCategoryRepository appointmentCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteAppointmentCategoryCommand>
{
    public async Task Handle(DeleteAppointmentCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
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
            
            // Deleting appointment category
            appointmentCategory.Delete(appointmentCategory.Appointments);
        }, cancellationToken);
    }
}