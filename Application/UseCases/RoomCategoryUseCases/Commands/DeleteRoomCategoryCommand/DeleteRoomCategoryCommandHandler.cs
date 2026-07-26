using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using MediatR;

namespace Application.UseCases.RoomCategoryUseCases.Commands.DeleteRoomCategoryCommand;

public class DeleteRoomCategoryCommandHandler(
    IRoomCategoryRepository roomCategoryRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteRoomCategoryCommand>
{
    public async Task Handle(DeleteRoomCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking room category
            var roomCategory = await roomCategoryRepository.GetByClinicIdAndRoomCategoryIdAsync(
                request.Clinic.Id, 
                request.Id, 
                cancellationToken,
                roomCategory => roomCategory.Rooms);
            if (roomCategory == null)
                throw new NotFoundException(nameof(RoomCategory), request.Id);
            
            // Deleting room category
            roomCategory.Delete(roomCategory.Rooms);
        }, cancellationToken);
    }
}