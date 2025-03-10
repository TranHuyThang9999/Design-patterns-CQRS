using WebApplicationCQRS.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAssignedTicket AssignedTicket { get; }
    IUserRepository UserRepository { get; }
    ITicketRepository TicketRepository { get; }
    IHistoryAssignTicketRepository HistoryAssignTicketRepository { get; }
    Task<int> SaveChangesAsync();
    Task ExecuteTransactionAsync(Func<Task> operation);

    public Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation,
        CancellationToken cancellationToken = default);
}