using Gamification.Domain.Awards.Models;

namespace Gamification.Domain.Awards.Ports;

/// <summary>
/// Porta para leitura de dados relacionados a premiações (lado Query)
/// </summary>
public interface IAwardsReadStore
{
    /// <summary>
    /// Obtém uma premiação pelo seu identificador único
    /// </summary>
    Task<Award?> GetByIdAsync(Guid awardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as premiações de um usuário específico
    /// </summary>
    Task<IReadOnlyList<Award>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se uma premiação existe para o critério informado
    /// </summary>
    Task<bool> ExistsAsync(Guid awardId, CancellationToken cancellationToken = default);
} 