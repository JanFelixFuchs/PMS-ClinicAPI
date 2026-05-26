using System.Linq.Expressions;
using Domain.Entities.DeviceEntities;

namespace Application.Repositories.DeviceRepositories;

public interface IDeviceCategoryRepository
{
    Task AddAsync(DeviceCategory deviceCategory, CancellationToken cancellationToken);
    
    Task<ICollection<DeviceCategory>> GetByClinicIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken,
        params Expression<Func<DeviceCategory, object>>[] includeProperties);
    
    Task<ICollection<DeviceCategory>> GetByClinicIdAndDeviceCategoryIdsAsync(
        Guid clinicId,
        ICollection<Guid> deviceCategoryIds,
        CancellationToken cancellationToken,
        params Expression<Func<DeviceCategory, object>>[] includeProperties);
    
    Task<DeviceCategory?> GetByClinicIdAndDeviceCategoryIdAsync(
        Guid clinicId,
        Guid deviceCategoryId,
        CancellationToken cancellationToken,
        params Expression<Func<DeviceCategory, object>>[] includeProperties);
}