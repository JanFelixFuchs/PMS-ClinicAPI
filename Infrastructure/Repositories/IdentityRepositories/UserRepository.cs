using System.Linq.Expressions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.IdentityRepositories;

public class UserRepository(DatabaseContext databaseContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            // Adding user
            await databaseContext.Users.AddAsync(user, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(User), exception);
        }
    }

    public async Task<ICollection<User>> GetByClinicIdAsync(
        Guid clinicId, 
        bool archived,
        CancellationToken cancellationToken, 
        params Expression<Func<User, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<User> query = databaseContext.Users;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying users
            return await query
                .Where(user => user.ClinicId == clinicId && user.IsArchived == archived)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception 
            throw new DatabaseException(nameof(User), exception);
        }
    }
    
    public async Task<ICollection<User>> GetByClinicIdAndUserIdsAsync(
        Guid clinicId, 
        ICollection<Guid> userIds, 
        CancellationToken cancellationToken, 
        params Expression<Func<User, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<User> query = databaseContext.Users;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying users
            return await query
                .Where(user => user.ClinicId == clinicId && userIds.Contains(user.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(User), exception);
        }
    }
    
    public async Task<User?> GetByClinicIdAndUserIdAsync(
        Guid clinicId,
        Guid userId,
        CancellationToken cancellationToken,
        params Expression<Func<User, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<User> query = databaseContext.Users;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying user
            return await query
                .Where(user => user.ClinicId == clinicId && user.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(User), exception);
        }
    }

    public async Task<User?> GetByClinicIdAndNormalizedUsernameAsync(
        Guid clinicId, 
        string normalizedUsername, 
        CancellationToken cancellationToken, 
        params Expression<Func<User, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<User> query = databaseContext.Users;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying user
            return await query
                .Where(user => user.ClinicId == clinicId && user.NormalizedUsername == normalizedUsername)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(User), exception);
        }
    }

    public async Task<User?> GetByRefreshTokenHashAsync(
        string refreshTokenHash, 
        CancellationToken cancellationToken,
        params Expression<Func<User, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<User> query = databaseContext.Users;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying user
            return await query
                .Where(user => user.RefreshTokenHash == refreshTokenHash)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(User), exception);
        }
    }
}