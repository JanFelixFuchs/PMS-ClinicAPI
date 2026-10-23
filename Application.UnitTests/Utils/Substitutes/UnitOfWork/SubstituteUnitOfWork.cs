using Application.Common.Transactions;

namespace Application.UnitTests.Utils.Substitutes.UnitOfWork;

public class SubstituteUnitOfWork : IUnitOfWork
{
    public int ExecutionCount { get; private set; }
    
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        ExecutionCount++;
        return await action();
    }
    
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        ExecutionCount++;
        await action();
    }
}