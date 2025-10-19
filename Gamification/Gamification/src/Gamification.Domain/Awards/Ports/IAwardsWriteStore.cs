using Gamification.Domain.Awards.Models;

namespace Gamification.Domain.Awards.Ports;

/// <summary>
/// Porta para gravação de dados relacionados a premiações (lado Command)
/// </summary>
public interface IAwardsWriteStore
{
    /// <summary>
    /// Cria uma nova premiação
    /// </summary>
    Task CreateAsync(Award award, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma premiação existente
    /// </summary>
    Task UpdateAsync(Award award, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma premiação pelo seu identificador
    /// </summary>
    Task DeleteAsync(Guid awardId, CancellationToken cancellationToken = default);

    // IAwardsWriteStore.cs (adicionar)
    Task CreateBadgeAwardAsync(BadgeAward badgeAward, Guid? requestId = null, CancellationToken ct = default);
}