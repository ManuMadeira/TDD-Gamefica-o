using System;
using System.Threading;
using System.Threading.Tasks;
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

    // Verifica se o estudante concluiu a missão (e se a missão existe)
    Task<bool> MissionCompletedAsync(Guid userId, Guid missionId, CancellationToken ct = default);

    // Verifica existência de badge por chave natural (userId, themeId, missionId, badgeSlug)
    Task<bool> BadgeExistsByNaturalKeyAsync(Guid userId, Guid themeId, Guid missionId, BadgeSlug badgeSlug, CancellationToken ct = default);

    // Checagem de idempotência por requestId (se a store persistir requestId)
    Task<Guid?> GetAwardIdByRequestIdAsync(Guid requestId, CancellationToken ct = default);

    // Obter datas de janelas de bônus do tema (ou retornar null se tema não existir)
    Task<(DateTimeOffset? bonusStart, DateTimeOffset? fullWeightEnd, DateTimeOffset? finalDate, int xpBase, int xpFullWeight, double xpReducedWeight)> GetThemeBonusPolicyAsync(Guid themeId, CancellationToken ct = default);
} 