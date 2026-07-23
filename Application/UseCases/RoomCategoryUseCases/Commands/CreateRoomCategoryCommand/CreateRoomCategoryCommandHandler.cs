using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.RoomCategoryUseCases.Commands.CreateRoomCategoryCommand;

public class CreateRoomCategoryCommandHandler(
    ILogger<CreateRoomCategoryCommandHandler> logger,
    IRoomRepository roomRepository,
    IRoomCategoryRepository roomCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateRoomCategoryCommand, RoomCategoryDetailedOutputModel>
{
    public async Task<RoomCategoryDetailedOutputModel> Handle(CreateRoomCategoryCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Creating room category
            var roomCategory = new RoomCategory(request.Clinic, request.Name, request.Abbreviation, request.Color);
            
            // Adding room category
            await roomCategoryRepository.AddAsync(roomCategory, cancellationToken);
            
            // Querying and checking rooms
            var rooms = await roomRepository.GetByClinicIdAndRoomIdsAsync(
                request.Clinic.Id, 
                request.RoomIds, 
                cancellationToken);
            var missingRoomIds = request.RoomIds.Except(rooms.Select(room => room.Id)).ToList();
            if (missingRoomIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(rooms), missingRoomIds);
                throw new NotFoundException(nameof(Room), missingRoomIds);
            }
            
            // Adding room category to rooms
            foreach (var room in rooms)
                room.AddRoomCategory(roomCategory);
            
            // Returning output model
            return new RoomCategoryDetailedOutputModel(roomCategory, rooms);
        }, cancellationToken); 
    }
}