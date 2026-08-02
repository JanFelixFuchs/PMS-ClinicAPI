using Application.Common.Exceptions;
using Application.Common.OutputModels.RoomOutputModels;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Queries.ReadRoomQuery;

public class ReadRoomQueryHandler(IRoomRepository roomRepository)
    : IRequestHandler<ReadRoomQuery, RoomDetailedOutputModel>
{
    public async Task<RoomDetailedOutputModel> Handle(ReadRoomQuery request, CancellationToken cancellationToken)
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
        
        // Returning output model
        return new RoomDetailedOutputModel(room, room.RoomCategories, room.Appointments, room.AppointmentProtocols);
    }
}