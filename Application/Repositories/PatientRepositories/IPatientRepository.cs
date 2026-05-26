using System.Linq.Expressions;
using Domain.Entities.PatientEntities;

namespace Application.Repositories.PatientRepositories;

public interface IPatientRepository
{
    Task AddAsync(Patient patient, CancellationToken cancellationToken);
    
    Task<ICollection<Patient>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Patient, object>>[] includeProperties);
    
    Task<Patient?> GetByClinicIdAndPatientIdAsync(
        Guid clinicId,
        Guid patientId,
        CancellationToken cancellationToken,
        params Expression<Func<Patient, object>>[] includeProperties);
}