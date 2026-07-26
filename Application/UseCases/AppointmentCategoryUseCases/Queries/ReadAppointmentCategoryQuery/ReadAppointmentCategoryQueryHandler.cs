using Application.Common.Exceptions;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;

namespace Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoryQuery;

public class ReadAppointmentCategoryQueryHandler(IAppointmentCategoryRepository appointmentCategoryRepository)
    : IRequestHandler<ReadAppointmentCategoryQuery, AppointmentCategoryDetailedOutputModel>
{
    public async Task<AppointmentCategoryDetailedOutputModel> Handle(ReadAppointmentCategoryQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking appointment category
        var appointmentCategory = await appointmentCategoryRepository.GetByClinicIdAndAppointmentCategoryIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken, 
            appointmentCategory => appointmentCategory.Appointments);
        if (appointmentCategory == null)
            throw new NotFoundException(nameof(AppointmentCategory), request.Id);
        
        // Returning output model
        return new AppointmentCategoryDetailedOutputModel(appointmentCategory, appointmentCategory.Appointments);
    }
}