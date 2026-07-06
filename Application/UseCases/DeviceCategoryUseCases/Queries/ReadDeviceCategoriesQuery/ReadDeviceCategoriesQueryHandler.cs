using Application.Common.OutputModels.DeviceOutputModels;
using Application.Repositories.DeviceRepositories;
using MediatR;

namespace Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoriesQuery;

public class ReadDeviceCategoriesQueryHandler(IDeviceCategoryRepository deviceCategoryRepository) 
    : IRequestHandler<ReadDeviceCategoriesQuery, List<DeviceCategoryOverviewOutputModel>>
{
    public async Task<List<DeviceCategoryOverviewOutputModel>> Handle(ReadDeviceCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Querying device categories
        var deviceCategories = await deviceCategoryRepository.GetByClinicIdAsync(
            request.Clinic.Id, 
            cancellationToken);
        
        // Returning output model
        return deviceCategories
            .Select(deviceCategory => new DeviceCategoryOverviewOutputModel(deviceCategory))
            .ToList();
    }
}