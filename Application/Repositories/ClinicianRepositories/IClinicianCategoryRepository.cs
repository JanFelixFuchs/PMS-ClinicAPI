using System.Linq.Expressions;
using Domain.Entities.ClinicianEntities;

namespace Application.Repositories.ClinicianRepositories;

public interface IClinicianCategoryRepository
{
    Task AddAsync(ClinicianCategory clinicianCategory, CancellationToken cancellationToken);
    
    Task<ICollection<ClinicianCategory>> GetByClinicIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken,
        params Expression<Func<ClinicianCategory, object>>[] includeProperties);
    
    Task<ICollection<ClinicianCategory>> GetByClinicIdAndClinicianCategoryIdsAsync(
        Guid clinicId,
        ICollection<Guid> clinicianCategoryIds,
        CancellationToken cancellationToken,
        params Expression<Func<ClinicianCategory, object>>[] includeProperties);
    
    Task<ClinicianCategory?> GetByClinicIdAndClinicianCategoryIdAsync(
        Guid clinicId,
        Guid clinicianCategoryId,
        CancellationToken cancellationToken,
        params Expression<Func<ClinicianCategory, object>>[] includeProperties);
}