using System.Linq.Expressions;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PatientRepositories;

public class PatientRepository(DatabaseContext databaseContext) : IPatientRepository
{
    public async Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        try
        {
            // Adding patient
            await databaseContext.Patients.AddAsync(patient, cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Patient), exception);
        }
    }

    public async Task<ICollection<Patient>> GetByClinicIdAsync(
        Guid clinicId,
        bool archived,
        CancellationToken cancellationToken,
        params Expression<Func<Patient, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Patient> query = databaseContext.Patients;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying patients
            return await query
                .Where(patient => patient.ClinicId == clinicId && patient.IsArchived == archived)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Patient), exception);
        }
    }
    
    public async Task<Patient?> GetByClinicIdAndPatientIdAsync(
        Guid clinicId, 
        Guid patientId, 
        CancellationToken cancellationToken,
        params Expression<Func<Patient, object>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Patient> query = databaseContext.Patients;
            
            // Including properties
            foreach (var property in includeProperties)
                query = query.Include(property);
            
            // Querying patients
            return await query
                .Where(patient => patient.ClinicId == clinicId && patient.Id == patientId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseException(nameof(Patient), exception);
        }
    }
}