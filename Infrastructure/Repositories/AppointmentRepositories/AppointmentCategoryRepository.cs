using System.Linq.Expressions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AppointmentRepositories;

public class AppointmentCategoryRepository(DatabaseContext databaseContext) : IAppointmentCategoryRepository
{
    public async Task AddAsync(AppointmentCategory appointmentCategory, CancellationToken cancellationToken)
    {
        try
        {
            // Adding appointment category
            await databaseContext.AppointmentCategories.AddAsync(appointmentCategory, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(AppointmentCategory), exception);
        }
    }

    public async Task<ICollection<AppointmentCategory>> GetByClinicIdAsync(
        Guid clinicId, 
        CancellationToken cancellationToken,
        params Expression<Func<AppointmentCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<AppointmentCategory> query = databaseContext.AppointmentCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying appointment categories
            return await query
                .Where(appointmentCategory => appointmentCategory.ClinicId == clinicId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(AppointmentCategory), exception);
        }
    }

    public async Task<ICollection<AppointmentCategory>> GetByClinicIdAndAppointmentCategoryIdsAsync(
        Guid clinicId, 
        ICollection<Guid> appointmentCategoryIds,
        CancellationToken cancellationToken, 
        params Expression<Func<AppointmentCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<AppointmentCategory> query = databaseContext.AppointmentCategories;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying appointment categories
            return await query
                .Where(appointmentCategory => appointmentCategory.ClinicId == clinicId && appointmentCategoryIds.Contains(appointmentCategory.Id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(AppointmentCategory), exception);
        }
    }

    public async Task<AppointmentCategory?> GetByClinicIdAndAppointmentCategoryIdAsync(
        Guid clinicId, 
        Guid appointmentCategoryId, 
        CancellationToken cancellationToken,
        params Expression<Func<AppointmentCategory, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<AppointmentCategory> query = databaseContext.AppointmentCategories;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying appointment category
            return await query
                .Where(appointmentCategory => appointmentCategory.ClinicId == clinicId && appointmentCategory.Id == appointmentCategoryId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(AppointmentCategory), exception);
        }
    }
}