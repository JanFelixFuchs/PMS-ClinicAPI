using Application.Common.Exceptions;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomCategoryUseCases.Queries.ReadRoomCategoryQuery;

public class ReadRoomCategoryQueryHandler(IRoomCategoryRepository roomCategoryRepository) 
    : IRequestHandler<ReadRoomCategoryQuery, RoomCategoryDetailedOutputModel>
{
    public async Task<RoomCategoryDetailedOutputModel> Handle(ReadRoomCategoryQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking room category
        var roomCategory = await roomCategoryRepository.GetByClinicIdAndRoomCategoryIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken, 
            roomCategory => roomCategory.Rooms);
        if (roomCategory == null)
            throw new NotFoundException(nameof(RoomCategory), request.Id);
        
        // Returning output model
        return new RoomCategoryDetailedOutputModel(roomCategory, roomCategory.Rooms);
    }
}