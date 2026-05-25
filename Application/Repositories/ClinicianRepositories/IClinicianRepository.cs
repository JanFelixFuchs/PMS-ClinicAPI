using System.Linq.Expressions;
using Domain.Entities.ClinicianEntities;

namespace Application.Repositories.ClinicianRepositories;

public interface IClinicianRepository
{
    Task AddAsync(Clinician clinician, CancellationToken cancellationToken);
    
    Task<ICollection<Clinician>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Clinician, object?>>[] includeProperties);
    
    Task<ICollection<Clinician>> GetByClinicIdAndClinicianIdsAsync(
        Guid clinicId,
        ICollection<Guid> clinicianIds,
        CancellationToken cancellationToken,
        params Expression<Func<Clinician, object?>>[] includeProperties);
    
    Task<Clinician?> GetByClinicIdAndClinicianIdAsync(
        Guid clinicId,
        Guid clinicianId,
        CancellationToken cancellationToken,
        params Expression<Func<Clinician, object?>>[] includeProperties);
}