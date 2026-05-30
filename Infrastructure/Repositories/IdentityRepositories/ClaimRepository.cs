using System.Linq.Expressions;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.IdentityRepositories;

public class ClaimRepository(DatabaseContext databaseContext) : IClaimRepository
{
    public async Task AddExceptValueEqualsNoneAsync(ICollection<Claim> claims, CancellationToken cancellationToken)
    {
        try
        {
            // Adding claims
            await databaseContext.Claims.AddRangeAsync(claims.Where(claim => claim.Value != ClaimValue.None), cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseCreateException(nameof(Claim), exception.Message);
        } 
    }
    
    public async Task<ICollection<Claim>> GetByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken,
        params Expression<Func<Claim, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Claim> query = databaseContext.Claims;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying claims
            return await query
                .Where(claim => claim.RoleId == roleId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Claim), exception.Message);
        }
    }
    
    public async Task DeleteByRoleIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        try
        {
            // Deleting claims
            await databaseContext.Claims
                .Where(claim => claim.RoleId == roleId)
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseDeleteException(nameof(Claim), exception.Message);
        }
    }
}