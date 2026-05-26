using System.Linq.Expressions;
using Domain.Entities.RoomEntities;

namespace Application.Repositories.RoomRepositories;

public interface IRoomRepository
{
    Task AddAsync(Room room, CancellationToken cancellationToken);
    
    Task<ICollection<Room>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Room, object>>[] includeProperties);
    
    Task<ICollection<Room>> GetByClinicIdAndRoomIdsAsync(
        Guid clinicId,
        ICollection<Guid> roomIds,
        CancellationToken cancellationToken,
        params Expression<Func<Room, object>>[] includeProperties);
    
    Task<Room?> GetByClinicIdAndRoomIdAsync(
        Guid clinicId,
        Guid roomId,
        CancellationToken cancellationToken,
        params Expression<Func<Room, object>>[] includeProperties);
}