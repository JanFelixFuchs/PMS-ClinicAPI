using System.Linq.Expressions;
using Application.Repositories.RoomRepositories;
using Domain.Entities.RoomEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RoomRepositories;

public class RoomRepository(DatabaseContext databaseContext) : IRoomRepository
{
    public async Task AddAsync(Room room, CancellationToken cancellationToken)
    {
        try
        {
            // Adding room
            await databaseContext.Rooms.AddAsync(room, cancellationToken);
        }
        catch (Exception exception)
        {   
            // Throwing exception
            throw new DatabaseCreateException(nameof(Room), exception.Message);
        }
    }

    public async Task<ICollection<Room>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Room, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Room> query = databaseContext.Rooms;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying rooms
            return await query
                .Where(room => room.ClinicId == clinicId && room.IsArchived == archived)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Room), exception.Message);
        }
    }
    
    public async Task<ICollection<Room>> GetByClinicIdAndRoomIdsAsync(
        Guid clinicId, 
        ICollection<Guid> roomIds, 
        CancellationToken cancellationToken,
        params Expression<Func<Room, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Room> query = databaseContext.Rooms;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying rooms
            return await query
                .Where(room => room.ClinicId == clinicId && roomIds.Contains(room.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Room), exception.Message);
        }
    }

    public async Task<Room?> GetByClinicIdAndRoomIdAsync(
        Guid clinicId, 
        Guid roomId, 
        CancellationToken cancellationToken,
        params Expression<Func<Room, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Room> query = databaseContext.Rooms;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying room
            return await query
                .Where(room => room.ClinicId == clinicId && room.Id == roomId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Room), exception.Message);
        }
    }
}