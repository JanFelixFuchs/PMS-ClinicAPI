using System.Linq.Expressions;
using Domain.Entities.AppointmentEntities;

namespace Application.Repositories.AppointmentRepositories;

public interface IAppointmentCategoryRepository
{
    Task AddAsync(
        AppointmentCategory appointmentCategory, 
        CancellationToken cancellationToken);
    
    Task<ICollection<AppointmentCategory>> GetByClinicIdAsync(
        Guid clinicId, 
        CancellationToken cancellationToken, 
        params Expression<Func<AppointmentCategory, object>>[] includeProperties);
    
    Task<ICollection<AppointmentCategory>> GetByClinicIdAndAppointmentCategoryIdsAsync(
        Guid clinicId,
        ICollection<Guid> appointmentCategoryIds,
        CancellationToken cancellationToken,
        params Expression<Func<AppointmentCategory, object>>[] includeProperties);
    
    Task<AppointmentCategory?> GetByClinicIdAndAppointmentCategoryIdAsync(
        Guid clinicId,
        Guid appointmentCategoryId,
        CancellationToken cancellationToken,
        params Expression<Func<AppointmentCategory, object>>[] includeProperties);
}