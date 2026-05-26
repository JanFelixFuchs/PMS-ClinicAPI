using System.Linq.Expressions;
using Domain.Entities.IdentityEntities;

namespace Application.Repositories.IdentityRepositories;

public interface IClinicRepository
{
    Task AddAsync(Clinic clinic, CancellationToken cancellationToken);
    
    Task<Clinic?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken,
        params Expression<Func<Clinic, object>>[] includeProperties);
    
    Task<Clinic?> GetByNormalizedCodeAsync(
        string normalizedCode,
        CancellationToken cancellationToken,
        params Expression<Func<Clinic, object>>[] includeProperties);
}