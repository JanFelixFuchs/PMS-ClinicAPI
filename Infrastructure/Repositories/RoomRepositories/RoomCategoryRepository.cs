using System.Linq.Expressions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RoomRepositories;

public class RoomCategoryRepository(DatabaseContext databaseContext) : IRoomCategoryRepository
{
    public async Task AddAsync(RoomCategory roomCategory, CancellationToken cancellationToken)
    {
        try
        {
            // Adding room category
            await databaseContext.RoomCategories.AddAsync(roomCategory, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(RoomCategory), exception);
        }
    }

    public async Task<ICollection<RoomCategory>> GetByClinicIdAsync(
        Guid clinicId, 
        CancellationToken cancellationToken,
        params Expression<Func<RoomCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<RoomCategory> query = databaseContext.RoomCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying room categories
            return await query
                .Where(roomCategory => roomCategory.ClinicId == clinicId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(RoomCategory), exception);
        }
    }

    public async Task<ICollection<RoomCategory>> GetByClinicIdAndRoomCategoryIdsAsync(
        Guid clinicId, 
        ICollection<Guid> roomCategoryIds,
        CancellationToken cancellationToken, 
        params Expression<Func<RoomCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<RoomCategory> query = databaseContext.RoomCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying room categories
            return await query
                .Where(roomCategory => roomCategory.ClinicId == clinicId && roomCategoryIds.Contains(roomCategory.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(RoomCategory), exception);
        }
    }
    
    public async Task<RoomCategory?> GetByClinicIdAndRoomCategoryIdAsync(
        Guid clinicId, 
        Guid roomCategoryId, 
        CancellationToken cancellationToken,
        params Expression<Func<RoomCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<RoomCategory> query = databaseContext.RoomCategories;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying room category
            return await query
                .Where(roomCategory => roomCategory.ClinicId == clinicId && roomCategory.Id == roomCategoryId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(RoomCategory), exception);
        }
    }
}