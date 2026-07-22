using System.Linq.Expressions;
using Application.Repositories.DeviceRepositories;
using Domain.Entities.DeviceEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.DeviceRepositories;

public class DeviceCategoryRepository(DatabaseContext databaseContext) : IDeviceCategoryRepository
{
    public async Task AddAsync(DeviceCategory deviceCategory, CancellationToken cancellationToken)
    {
        try
        {
            // Adding device category
            await databaseContext.DeviceCategories.AddAsync(deviceCategory, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(DeviceCategory), exception);
        }
    }

    public async Task<ICollection<DeviceCategory>> GetByClinicIdAsync(
        Guid clinicId, 
        CancellationToken cancellationToken,
        params Expression<Func<DeviceCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<DeviceCategory> query = databaseContext.DeviceCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying device categories
            return await query
                .Where(deviceCategory => deviceCategory.ClinicId == clinicId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(DeviceCategory), exception);
        }
    }

    public async Task<ICollection<DeviceCategory>> GetByClinicIdAndDeviceCategoryIdsAsync(
        Guid clinicId, 
        ICollection<Guid> deviceCategoryIds,
        CancellationToken cancellationToken, 
        params Expression<Func<DeviceCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<DeviceCategory> query = databaseContext.DeviceCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying device categories
            return await query
                .Where(deviceCategory => deviceCategory.ClinicId == clinicId && deviceCategoryIds.Contains(deviceCategory.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(DeviceCategory), exception);
        }
    }

    public async Task<DeviceCategory?> GetByClinicIdAndDeviceCategoryIdAsync(
        Guid clinicId, 
        Guid deviceCategoryId, 
        CancellationToken cancellationToken,
        params Expression<Func<DeviceCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<DeviceCategory> query = databaseContext.DeviceCategories;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying device category
            return await query
                .Where(deviceCategory => deviceCategory.ClinicId == clinicId && deviceCategory.Id == deviceCategoryId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(DeviceCategory), exception);
        }
    }
}