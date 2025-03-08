namespace WebApplicationCQRS.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);

}