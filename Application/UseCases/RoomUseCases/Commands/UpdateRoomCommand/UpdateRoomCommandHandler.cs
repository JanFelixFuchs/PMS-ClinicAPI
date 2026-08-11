using Application.Common.Exceptions;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Commands.UpdateRoomCommand;

public class UpdateRoomCommandHandler(
    IRoomCategoryRepository roomCategoryRepository,
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRoomCommand, RoomDetailedOutputModel>
{
    public async Task<RoomDetailedOutputModel> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking room
            var room = await roomRepository.GetByClinicIdAndRoomIdAsync(
                request.Clinic.Id,
                request.Id,
                cancellationToken,
                room => room.RoomCategories,
                room => room.Appointments,
                room => room.AppointmentProtocols);
            if (room == null)
                throw new NotFoundException(nameof(Room), request.Id);
            
            // Querying and checking room categories
            var roomCategories = await roomCategoryRepository.GetByClinicIdAndRoomCategoryIdsAsync(
                request.Clinic.Id,
                request.RoomCategoryIds,
                cancellationToken);
            var missingRoomCategoryIds = request.RoomCategoryIds.Except(roomCategories.Select(roomCategory => roomCategory.Id)).ToList();
            if (missingRoomCategoryIds.Count > 0)
                throw new NotFoundException(nameof(RoomCategory), missingRoomCategoryIds);
            
            // Updating room
            room.Update(
                request.Name, 
                request.Abbreviation, 
                roomCategories, 
                request.RoomNumber, 
                request.Floor, 
                request.Building);
            
            // Returning output model
            return new RoomDetailedOutputModel(room, room.RoomCategories, room.Appointments, room.AppointmentProtocols);
        }, cancellationToken);
    }
}