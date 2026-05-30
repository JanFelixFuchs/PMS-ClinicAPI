using System.Linq.Expressions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.IdentityRepositories;

public class RoleRepository(DatabaseContext databaseContext) : IRoleRepository
{
    public async Task AddAsync(Role role, CancellationToken cancellationToken)
    {
        try
        {
            // Adding role
            await databaseContext.Roles.AddAsync(role, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseCreateException(nameof(Role), exception.Message);
        }
    }
    
    public async Task<ICollection<Role>> GetByClinicIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken,
        params Expression<Func<Role, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Role> query = databaseContext.Roles;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying roles
            return await query
                .Where(role => role.ClinicId == clinicId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Role), exception.Message);
        }
    }

    public async Task<Role?> GetByClinicIdAndRoleIdAsync(
        Guid clinicId,
        Guid roleId,
        CancellationToken cancellationToken,
        params Expression<Func<Role, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Role> query = databaseContext.Roles;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying role
            return await query
                .Where(role => role.ClinicId == clinicId && role.Id == roleId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Role), exception.Message);
        }
    }
    
    public async Task<Role?> GetByClinicIdAndNormalizedNameAsync(
        Guid clinicId,
        string normalizedName,
        CancellationToken cancellationToken,
        params Expression<Func<Role, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Role> query = databaseContext.Roles;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying role
            return await query.
                Where(role => role.ClinicId == clinicId && role.NormalizedName == normalizedName)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Role), exception.Message);
        }
    }
}