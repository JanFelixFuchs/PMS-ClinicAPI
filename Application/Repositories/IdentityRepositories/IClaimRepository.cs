using System.Linq.Expressions;
using Claim = Domain.Entities.IdentityEntities.Claim;

namespace Application.Repositories.IdentityRepositories;

public interface IClaimRepository
{
    Task AddExceptValueEqualsNoneAsync(ICollection<Claim> claims, CancellationToken cancellationToken);
    
    Task<ICollection<Claim>> GetByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken,
        params Expression<Func<Claim, object>>[] includeProperties);
    
    Task DeleteByRoleIdAsync(Guid roleId, CancellationToken cancellationToken);
}