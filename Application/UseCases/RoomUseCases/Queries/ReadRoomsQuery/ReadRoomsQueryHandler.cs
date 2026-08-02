using Application.Common.OutputModels.RoomOutputModels;
using Application.Repositories.RoomRepositories;
using MediatR;

namespace Application.UseCases.RoomUseCases.Queries.ReadRoomsQuery;

public class ReadRoomsQueryHandler(IRoomRepository roomRepository) 
    : IRequestHandler<ReadRoomsQuery, List<RoomOverviewOutputModel>>
{
    public async Task<List<RoomOverviewOutputModel>> Handle(ReadRoomsQuery request, CancellationToken cancellationToken)
    {
        // Querying rooms
        var rooms = await roomRepository.GetByClinicIdAsync(
            request.Clinic.Id,
            request.Archived,
            cancellationToken);
        
        // Returning output model
        return rooms
            .Select(room => new RoomOverviewOutputModel(room))
            .ToList();
    }
}