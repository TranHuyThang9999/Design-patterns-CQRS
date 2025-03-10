using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Persistence.Context;

namespace WebApplicationCQRS.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IAssignedTicket AssignedTicket { get; }
    public IUserRepository UserRepository { get; }
    public ITicketRepository TicketRepository { get; }
    public IHistoryAssignTicketRepository HistoryAssignTicketRepository { get; }

    public UnitOfWork(AppDbContext context, IAssignedTicket assignedTicket, IUserRepository userRepository,
        ITicketRepository ticketRepository, IHistoryAssignTicketRepository historyAssignTicketRepository)
    {
        _context = context;
        AssignedTicket = assignedTicket;
        UserRepository = userRepository;
        TicketRepository = ticketRepository;
        HistoryAssignTicketRepository = historyAssignTicketRepository;
    }
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation();
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return default!;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task ExecuteTransactionAsync(Func<Task> operation)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await operation();
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}