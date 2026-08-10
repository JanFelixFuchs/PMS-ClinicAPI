using Application.Common.Exceptions;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Commands.UnarchiveRoomCommand;

public class UnarchiveRoomCommandHandler(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UnarchiveRoomCommand, RoomDetailedOutputModel>
{
    public async Task<RoomDetailedOutputModel> Handle(UnarchiveRoomCommand request, CancellationToken cancellationToken)
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
            
            // Unarchiving room
            room.Unarchive();
            
            // Returning output model
            return new RoomDetailedOutputModel(room, room.RoomCategories, room.Appointments, room.AppointmentProtocols);
        }, cancellationToken);
    }
}