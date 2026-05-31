using System.Linq.Expressions;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using Infrastructure.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AppointmentRepositories;

public class ResultRepository(DatabaseContext databaseContext) : IResultRepository
{
    public async Task AddAsync(Result result, CancellationToken cancellationToken)
    {
        try
        {
            // Adding result
            await databaseContext.Results.AddAsync(result, cancellationToken);
        }
        catch (Exception exception)
        {   
            // Throwing exception
            throw new DatabaseCreateException(nameof(Result), exception.Message);
        }
    }

    public async Task<Result?> GetByClinicIdAndResultIdAsync(
        Guid clinicId,
        Guid resultId,
        CancellationToken cancellationToken,
        params Expression<Func<Result, object?>>[] includeProperties)
    {
        try
        {
            // Initializing query
            IQueryable<Result> query = databaseContext.Results;
            
            // Including properties
            foreach (var include in includeProperties) 
                query = query.Include(include);
            
            // Querying result
            return await query
                .Where(result => result.ClinicId == clinicId && result.Id == resultId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            // Throwing exception
            throw new DatabaseReadException(nameof(Result), exception.Message);
        }
    }
}