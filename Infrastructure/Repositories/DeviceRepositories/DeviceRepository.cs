using System.Linq.Expressions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.DeviceRepositories;

public class DeviceRepository(DatabaseContext databaseContext) : IDeviceRepository
{
    public async Task AddAsync(Device device, CancellationToken cancellationToken)
    {
        try
        {
            // Adding device
            await databaseContext.Devices.AddAsync(device, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseCreateException(nameof(Device), exception.Message);
        }
    }

    public async Task<ICollection<Device>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Device, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Device> query = databaseContext.Devices;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying devices
            return await query
                .Where(device => device.ClinicId == clinicId && device.IsArchived == archived)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Device), exception.Message);
        }
    }
    
    public async Task<ICollection<Device>> GetByClinicIdAndDeviceIdsAsync(
        Guid clinicId, 
        ICollection<Guid> deviceIds, 
        CancellationToken cancellationToken,
        params Expression<Func<Device, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Device> query = databaseContext.Devices;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying devices
            return await query
                .Where(device => device.ClinicId == clinicId && deviceIds.Contains(device.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Device), exception.Message);
        }
    }

    public async Task<Device?> GetByClinicIdAndDeviceIdAsync(
        Guid clinicId, 
        Guid deviceId, 
        CancellationToken cancellationToken,
        params Expression<Func<Device, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Device> query = databaseContext.Devices;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying device
            return await query
                .Where(device => device.ClinicId == clinicId && device.Id == deviceId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Device), exception.Message);
        }
    }
}