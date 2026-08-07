using Application.Common.Exceptions;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Commands.CreateRoomCommand;

public class CreateRoomCommandHandler(
    IRoomCategoryRepository roomCategoryRepository,
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateRoomCommand, RoomDetailedOutputModel>
{
    public async Task<RoomDetailedOutputModel> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking room categories
            var roomCategories = await roomCategoryRepository.GetByClinicIdAndRoomCategoryIdsAsync(
                request.Clinic.Id,
                request.RoomCategoryIds,
                cancellationToken);
            var missingRoomCategoryIds = request.RoomCategoryIds.Except(roomCategories.Select(roomCategory => roomCategory.Id)).ToList();
            if (missingRoomCategoryIds.Count > 0)
                throw new NotFoundException(nameof(RoomCategory), missingRoomCategoryIds);
            
            // Creating room
            var room = new Room(
                request.Clinic,
                request.Name,
                request.Abbreviation,
                roomCategories,
                request.RoomNumber,
                request.Floor,
                request.Building
            );

            // Adding room
            await roomRepository.AddAsync(room, cancellationToken);
            
            // Returning output model
            return new RoomDetailedOutputModel(room, roomCategories, room.Appointments, room.AppointmentProtocols);
        }, cancellationToken);
    }
}