using System.Linq.Expressions;
using Domain.Entities.IdentityEntities;

namespace Application.Repositories.IdentityRepositories;

public interface IUserRepository
{ 
    Task AddAsync(User user, CancellationToken cancellationToken);
    
    Task<ICollection<User>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<User, object?>>[] includeProperties);
    
    Task<ICollection<User>> GetByClinicIdAndUserIdsAsync(
        Guid clinicId,
        ICollection<Guid> userIds,
        CancellationToken cancellationToken,
        params Expression<Func<User, object?>>[] includeProperties);
    
    Task<User?> GetByClinicIdAndUserIdAsync(
        Guid clinicId,
        Guid userId,
        CancellationToken cancellationToken,
        params Expression<Func<User, object?>>[] includeProperties);
    
    Task<User?> GetByClinicIdAndNormalizedUsernameAsync(
        Guid clinicId,
        string normalizedUsername,
        CancellationToken cancellationToken,
        params Expression<Func<User, object?>>[] includeProperties);
}
