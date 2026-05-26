using System.Linq.Expressions;
using Domain.Entities.RoomEntities;

namespace Application.Repositories.RoomRepositories;

public interface IRoomCategoryRepository
{
    Task AddAsync(RoomCategory roomCategory, CancellationToken cancellationToken);
    
    Task<ICollection<RoomCategory>> GetByClinicIdAsync(
        Guid clinicId, 
        CancellationToken cancellationToken, 
        params Expression<Func<RoomCategory, object>>[] includeProperties);
    
    Task<ICollection<RoomCategory>> GetByClinicIdAndRoomCategoryIdsAsync(
        Guid clinicId,
        ICollection<Guid> roomCategoryIds,
        CancellationToken cancellationToken,
        params Expression<Func<RoomCategory, object>>[] includeProperties);
    
    Task<RoomCategory?> GetByClinicIdAndRoomCategoryIdAsync(
        Guid clinicId,
        Guid roomCategoryId,
        CancellationToken cancellationToken,
        params Expression<Func<RoomCategory, object>>[] includeProperties);
}