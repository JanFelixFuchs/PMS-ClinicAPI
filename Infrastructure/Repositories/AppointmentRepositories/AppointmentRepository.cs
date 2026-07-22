using System.Linq.Expressions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AppointmentRepositories;

public class AppointmentRepository(DatabaseContext databaseContext) : IAppointmentRepository
{
    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        try
        {
            // Adding appointment
            await databaseContext.Appointments.AddAsync(appointment, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Appointment), exception);
        }
    }

    public async Task<ICollection<Appointment>> GetOverlappingByClinicIdAndDateTimesAsync(
        Guid clinicId, 
        DateTime startDateTime, 
        DateTime endDateTime,
        CancellationToken cancellationToken, 
        params Expression<Func<Appointment, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Appointment> query = databaseContext.Appointments;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying appointments
            return await query
                .Where(
                    appointment => appointment.ClinicId == clinicId && 
                    appointment.StartTime < endDateTime && 
                    appointment.EndTime > startDateTime)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Appointment), exception);
        }
    }
    
    public async Task<ICollection<Appointment>> GetByClinicIdAndDateTimesAsync(
        Guid clinicId, 
        DateTime startDateTime, 
        DateTime endDateTime,
        CancellationToken cancellationToken, 
        params Expression<Func<Appointment, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Appointment> query = databaseContext.Appointments;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying appointments
            return await query
                .Where(
                    appointment => appointment.ClinicId == clinicId && 
                                   appointment.StartTime >= startDateTime && 
                                   appointment.EndTime <= endDateTime)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Appointment), exception);
        }
    }
    
    public async Task<Appointment?> GetByClinicIdAndAppointmentIdAsync(
        Guid clinicId, 
        Guid appointmentId, 
        CancellationToken cancellationToken,
        params Expression<Func<Appointment, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Appointment> query = databaseContext.Appointments;
            
            // Including properties
            foreach (var include in includeProperties)
                query = query.Include(include);
            
            // Querying appointment
            return await query
                .Where(appointment => appointment.ClinicId == clinicId && appointment.Id == appointmentId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Appointment), exception);
        }
    }
}