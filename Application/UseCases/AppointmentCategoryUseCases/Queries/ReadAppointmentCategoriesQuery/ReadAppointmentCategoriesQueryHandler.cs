using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Repositories.AppointmentRepositories;
using MediatR;

namespace Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoriesQuery;

public class ReadAppointmentCategoriesQueryHandler(IAppointmentCategoryRepository appointmentCategoryRepository)
    : IRequestHandler<ReadAppointmentCategoriesQuery, List<AppointmentCategoryOverviewOutputModel>>
{
    public async Task<List<AppointmentCategoryOverviewOutputModel>> Handle(ReadAppointmentCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Querying appointment categories
        var appointmentCategories = await appointmentCategoryRepository.GetByClinicIdAsync(
            request.Clinic.Id, 
            cancellationToken);
        
        // Returning output model
        return appointmentCategories
            .Select(appointmentCategory => new AppointmentCategoryOverviewOutputModel(appointmentCategory))
            .ToList();
    }
}