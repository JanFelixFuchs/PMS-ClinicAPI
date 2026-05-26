using System.Linq.Expressions;
using Domain.Entities.IdentityEntities;

namespace Application.Repositories.IdentityRepositories;

public interface IRoleRepository
{
    Task AddAsync(Role role, CancellationToken cancellationToken);
    
    Task<ICollection<Role>> GetByClinicIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken,
        params Expression<Func<Role, object>>[] includeProperties);
    
    Task<Role?> GetByClinicIdAndRoleIdAsync(
        Guid clinicId,
        Guid roleId,
        CancellationToken cancellationToken,
        params Expression<Func<Role, object>>[] includeProperties);
    
    Task<Role?> GetByClinicIdAndNormalizedNameAsync(
        Guid clinicId,
        string normalizedName,
        CancellationToken cancellationToken,
        params Expression<Func<Role, object>>[] includeProperties);
}