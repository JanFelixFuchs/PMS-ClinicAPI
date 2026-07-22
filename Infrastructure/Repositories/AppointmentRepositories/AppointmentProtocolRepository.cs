using System.Linq.Expressions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AppointmentRepositories;

public class AppointmentProtocolRepository(DatabaseContext databaseContext) : IAppointmentProtocolRepository
{
    public async Task AddAsync(AppointmentProtocol appointmentProtocol, CancellationToken cancellationToken)
    {
        try
        {
            // Adding appointment protocols
            await databaseContext.AppointmentProtocols.AddAsync(appointmentProtocol, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(AppointmentProtocol), exception);
        }
    }
    
    public async Task<AppointmentProtocol?> GetByClinicIdAndAppointmentProtocolIdAsync(
        Guid clinicId, 
        Guid appointmentProtocolId,
        CancellationToken cancellationToken, 
        params Expression<Func<AppointmentProtocol, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<AppointmentProtocol> query = databaseContext.AppointmentProtocols;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying appointment protocol
            return await query
                .Where(appointmentProtocol => appointmentProtocol.ClinicId == clinicId && appointmentProtocol.Id == appointmentProtocolId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(AppointmentProtocol), exception);
        }
    }
}