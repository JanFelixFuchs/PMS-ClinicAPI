using System.Linq.Expressions;
using Domain.Entities.AppointmentEntities;

namespace Application.Repositories.AppointmentRepositories;

public interface IAppointmentProtocolRepository
{
    Task AddAsync(AppointmentProtocol appointmentProtocol, CancellationToken cancellationToken);
    Task<AppointmentProtocol?> GetByClinicIdAndAppointmentProtocolIdAsync(
        Guid clinicId,
        Guid appointmentProtocolId,
        CancellationToken cancellationToken,
        params Expression<Func<AppointmentProtocol, object>>[] includeProperties);
}