using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomUseCases.Commands.DeleteRoomCommand;

public class DeleteRoomCommandHandler(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking room
            var room = await roomRepository.GetByClinicIdAndRoomIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                room => room.Appointments,
                room => room.AppointmentProtocols);
            if (room == null)
                throw new NotFoundException(nameof(Room), request.Id);
            
            // Deleting room
            room.Delete(room.Appointments, room.AppointmentProtocols);
        }, cancellationToken);
    }
}