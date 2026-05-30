using System.Linq.Expressions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.IdentityRepositories;

public class ClinicRepository(DatabaseContext databaseContext) : IClinicRepository
{
    public async Task AddAsync(Clinic clinic, CancellationToken cancellationToken)
    {
        try
        {
            // Adding clinic
            await databaseContext.Clinics.AddAsync(clinic, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseCreateException(nameof(Clinic), exception.Message);
        }
    }
    
    public async Task<Clinic?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken,
        params Expression<Func<Clinic, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Clinic> query = databaseContext.Clinics;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying clinic
            return await query
                .Where(clinic => clinic.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Clinic), exception.Message);
        }
    }

    public async Task<Clinic?> GetByNormalizedCodeAsync(
        string normalizedCode,
        CancellationToken cancellationToken,
        params Expression<Func<Clinic, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Clinic> query = databaseContext.Clinics;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying clinic
            return await query
                .Where(clinic => clinic.NormalizedCode == normalizedCode)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Clinic), exception.Message);
        }
    }
}