using System.Linq.Expressions;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ClinicianRepositories;

public class ClinicianRepository(DatabaseContext databaseContext) : IClinicianRepository
{
    public async Task AddAsync(Clinician clinician, CancellationToken cancellationToken)
    {
        try
        {
            // Adding clinician
            await databaseContext.Clinicians.AddAsync(clinician, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseCreateException(nameof(Clinician), exception.Message);
        }
    }

    public async Task<ICollection<Clinician>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Clinician, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Clinician> query = databaseContext.Clinicians;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying clinicians
            return await query
                .Where(clinician => clinician.ClinicId == clinicId && clinician.IsArchived == archived)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Clinician), exception.Message);
        }
    }
    
    public async Task<ICollection<Clinician>> GetByClinicIdAndClinicianIdsAsync(
        Guid clinicId, 
        ICollection<Guid> clinicianIds, 
        CancellationToken cancellationToken,
        params Expression<Func<Clinician, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Clinician> query = databaseContext.Clinicians;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying clinicians
            return await query
                .Where(clinician => clinician.ClinicId == clinicId && clinicianIds.Contains(clinician.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Clinician), exception.Message);
        }
    }

    public async Task<Clinician?> GetByClinicIdAndClinicianIdAsync(
        Guid clinicId, 
        Guid clinicianId, 
        CancellationToken cancellationToken,
        params Expression<Func<Clinician, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Clinician> query = databaseContext.Clinicians;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying clinician
            return await query
                .Where(clinician => clinician.ClinicId == clinicId && clinician.Id == clinicianId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Clinician), exception.Message);
        }
    }
}