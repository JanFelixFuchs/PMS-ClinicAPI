namespace Application.Common.Transactions;

public interface IUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken);
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken);
}