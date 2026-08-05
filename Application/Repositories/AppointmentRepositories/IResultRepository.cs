using System.Linq.Expressions;
using Domain.Entities.AppointmentEntities;

namespace Application.Repositories.AppointmentRepositories;

public interface IResultRepository
{
    // Methods
    Task AddAsync(Result result, CancellationToken cancellationToken);
    
    Task<Result?> GetByClinicIdAndResultIdAsync(
        Guid clinicId,
        Guid resultId,
        CancellationToken cancellationToken,
        params Expression<Func<Result, object?>>[] includeProperties);
}