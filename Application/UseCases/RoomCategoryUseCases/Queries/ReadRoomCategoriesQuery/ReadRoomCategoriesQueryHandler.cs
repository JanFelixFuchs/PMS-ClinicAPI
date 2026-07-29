using Application.Common.OutputModels.RoomOutputModels;
using Application.Repositories.RoomRepositories;
using MediatR;

namespace Application.UseCases.RoomCategoryUseCases.Queries.ReadRoomCategoriesQuery;

public class ReadRoomCategoriesQueryHandler(IRoomCategoryRepository roomCategoryRepository) 
    : IRequestHandler<ReadRoomCategoriesQuery, List<RoomCategoryOverviewOutputModel>>
{
    public async Task<List<RoomCategoryOverviewOutputModel>> Handle(ReadRoomCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Querying room categories
        var roomCategories = await roomCategoryRepository.GetByClinicIdAsync(
            request.Clinic.Id, 
            cancellationToken);
        
        // Returning output model
        return roomCategories
            .Select(roomCategory => new RoomCategoryOverviewOutputModel(roomCategory))
            .ToList();
    }
}