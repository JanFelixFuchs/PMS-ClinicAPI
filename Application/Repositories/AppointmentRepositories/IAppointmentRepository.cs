using System.Linq.Expressions;
using Domain.Entities.AppointmentEntities;

namespace Application.Repositories.AppointmentRepositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken);
    
    Task<ICollection<Appointment>> GetOverlappingByClinicIdAndDateTimesAsync(
        Guid clinicId, 
        DateTime startDateTime,
        DateTime endDateTime,
        CancellationToken cancellationToken, 
        params Expression<Func<Appointment, object?>>[] includeProperties);

    Task<ICollection<Appointment>> GetByClinicIdAndDateTimesAsync(
        Guid clinicId,
        DateTime startDateTime,
        DateTime endDateTime,
        CancellationToken cancellationToken,
        params Expression<Func<Appointment, object?>>[] includeProperties);
    
    Task<Appointment?> GetByClinicIdAndAppointmentIdAsync(
        Guid clinicId, 
        Guid appointmentId, 
        CancellationToken cancellationToken, 
        params Expression<Func<Appointment, object?>>[] includeProperties);
}