using Application.Common.Exceptions;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomCategoryUseCases.Commands.UpdateRoomCategoryCommand;

public class UpdateRoomCategoryCommandHandler(
    IRoomRepository roomRepository,
    IRoomCategoryRepository roomCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateRoomCategoryCommand, RoomCategoryDetailedOutputModel>
{
    public async Task<RoomCategoryDetailedOutputModel> Handle(UpdateRoomCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking room category
            var roomCategory = await roomCategoryRepository.GetByClinicIdAndRoomCategoryIdAsync(
                request.Clinic.Id,
                request.Id,
                cancellationToken,
                roomCategory => roomCategory.Rooms);
            if (roomCategory == null)
                throw new NotFoundException(nameof(RoomCategory), request.Id);
            
            // Updating room category
            roomCategory.Update(request.Name, request.Abbreviation, request.Color);
            
            // Defining current rooms
            var currentRooms = roomCategory.Rooms.ToList();
            var currentRoomIds = currentRooms.Select(room => room.Id).ToHashSet();
            
            // Calculating unchanged rooms
            var unchangedRooms = currentRooms.Where(room => request.RoomIds.Contains(room.Id)).ToList();
            
            // Calculating rooms to be changed
            var roomIdsToRemoveFrom = currentRoomIds.Except(request.RoomIds).ToHashSet();
            var roomIdsToAddTo = request.RoomIds.Except(currentRoomIds).ToHashSet();
            
            // Removing room categories from non-assigned rooms
            var roomsToRemoveFrom = currentRooms.Where(room => roomIdsToRemoveFrom.Contains(room.Id)).ToList();
            foreach (var room in roomsToRemoveFrom)
                room.RemoveRoomCategory(roomCategory);

            // Adding room categories to newly assigned rooms
            ICollection<Room> roomsToAddTo = new List<Room>();
            if (roomIdsToAddTo.Count > 0)
            {
                roomsToAddTo = await roomRepository.GetByClinicIdAndRoomIdsAsync(
                    request.Clinic.Id,
                    roomIdsToAddTo,
                    cancellationToken);
                var missingRoomIds = roomIdsToAddTo.Except(roomsToAddTo.Select(room => room.Id)).ToList();
                if (missingRoomIds.Count > 0)
                    throw new NotFoundException(nameof(Room), missingRoomIds);
        
                foreach (var room in roomsToAddTo)
                    room.AddRoomCategory(roomCategory);
            }
            
            // Returning output model
            return new RoomCategoryDetailedOutputModel(roomCategory, unchangedRooms.Concat(roomsToAddTo).ToList());
        }, cancellationToken);
    }
}