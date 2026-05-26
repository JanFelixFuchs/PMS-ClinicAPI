using System.Linq.Expressions;
using Domain.Entities.DeviceEntities;

namespace Application.Repositories.DeviceRepositories;

public interface IDeviceRepository
{
    Task AddAsync(Device device, CancellationToken cancellationToken);

    Task<ICollection<Device>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Device, object>>[] includeProperties);
    
    Task<ICollection<Device>> GetByClinicIdAndDeviceIdsAsync(
        Guid clinicId,
        ICollection<Guid> deviceIds,
        CancellationToken cancellationToken,
        params Expression<Func<Device, object>>[] includeProperties);
    
    Task<Device?> GetByClinicIdAndDeviceIdAsync(
        Guid clinicId,
        Guid deviceId,
        CancellationToken cancellationToken,
        params Expression<Func<Device, object>>[] includeProperties);
}
