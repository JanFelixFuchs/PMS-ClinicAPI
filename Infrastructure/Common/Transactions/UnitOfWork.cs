using Application.Common.Transactions;

namespace Infrastructure.Common.Transactions;

public class UnitOfWork(DatabaseContext databaseContext) : IUnitOfWork
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        // Initializing transaction
        await using var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Executing action
            var result = await action();
            
            // Saving changes and commiting
            await databaseContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            // Returning result
            return result;
        }
        catch (Exception)
        {
            // Executing rollback
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        // Executing transaction
        await ExecuteAsync(async () =>
        {
            // Executing action
            await action();
            
            // Returning result
            return true;
        }, cancellationToken);
    }
}