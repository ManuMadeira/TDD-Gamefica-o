namespace Gamification.Domain.Awards.Ports;

/// <summary>
/// Padrão Unit of Work para gerenciar operações relacionadas a premiações de forma atômica
/// </summary>
public interface IAwardsUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Repositório para leitura de dados de premiações
    /// </summary>
    IAwardsReadStore ReadStore { get; }

    /// <summary>
    /// Repositório para gravação de dados de premiações
    /// </summary>
    IAwardsWriteStore WriteStore { get; }

    /// <summary>
    /// Persiste todas as alterações realizadas nesta unidade de trabalho
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia uma transação
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma a transação atual
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Desfaz (rollback) a transação atual
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
