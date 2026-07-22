using System.Linq.Expressions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ClinicianRepositories;

public class ClinicianCategoryRepository(DatabaseContext databaseContext) : IClinicianCategoryRepository
{
    public async Task AddAsync(ClinicianCategory clinicianCategory, CancellationToken cancellationToken)
    {
        try
        {
            // Adding clinician category
            await databaseContext.ClinicianCategories.AddAsync(clinicianCategory, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(ClinicianCategory), exception);
        }
    }

    public async Task<ICollection<ClinicianCategory>> GetByClinicIdAsync(
        Guid clinicId, 
        CancellationToken cancellationToken,
        params Expression<Func<ClinicianCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<ClinicianCategory> query = databaseContext.ClinicianCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying clinician categories
            return await query
                .Where(clinicianCategory => clinicianCategory.ClinicId == clinicId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(ClinicianCategory), exception);
        }
    }

    public async Task<ICollection<ClinicianCategory>> GetByClinicIdAndClinicianCategoryIdsAsync(
        Guid clinicId, 
        ICollection<Guid> clinicianCategoryIds,
        CancellationToken cancellationToken, 
        params Expression<Func<ClinicianCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<ClinicianCategory> query = databaseContext.ClinicianCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying clinician categories
            return await query
                .Where(clinicianCategory => clinicianCategory.ClinicId == clinicId && clinicianCategoryIds.Contains(clinicianCategory.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(ClinicianCategory), exception);
        }
    }

    public async Task<ClinicianCategory?> GetByClinicIdAndClinicianCategoryIdAsync(
        Guid clinicId, 
        Guid clinicianCategoryId, 
        CancellationToken cancellationToken,
        params Expression<Func<ClinicianCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<ClinicianCategory> query = databaseContext.ClinicianCategories;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying clinician category
            return await query
                .Where(clinicianCategory => clinicianCategory.ClinicId == clinicId && clinicianCategory.Id == clinicianCategoryId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(ClinicianCategory), exception);
        }
    }
}