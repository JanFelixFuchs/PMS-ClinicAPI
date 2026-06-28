using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Repositories.ClinicianRepositories;
using MediatR;

namespace Application.UseCases.ClinicianCategoryUseCases.Queries.ReadClinicianCategoriesQuery;

public class ReadClinicianCategoriesQueryHandler(IClinicianCategoryRepository clinicianCategoryRepository) 
    : IRequestHandler<ReadClinicianCategoriesQuery, List<ClinicianCategoryOverviewOutputModel>>
{
    public async Task<List<ClinicianCategoryOverviewOutputModel>> Handle(ReadClinicianCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Querying clinician categories
        var clinicianCategories = await clinicianCategoryRepository.GetByClinicIdAsync(
            request.Clinic.Id, 
            cancellationToken);
        
        // Returning output model
        return clinicianCategories
            .Select(clinicianCategory => new ClinicianCategoryOverviewOutputModel(clinicianCategory))
            .ToList();
    }
}