namespace Gamification.Domain.Awards.Ports;

/// <summary>
/// Unit of Work pattern for managing award-related operations atomically
/// </summary>
public interface IAwardsUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Repository for reading award data
    /// </summary>
    IAwardsReadStore ReadStore { get; }

    /// <summary>
    /// Repository for writing award data
    /// </summary>
    IAwardsWriteStore WriteStore { get; }

    /// <summary>
    /// Commits all changes made in this unit of work
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
