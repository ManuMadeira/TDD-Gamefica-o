using System;

namespace Gamification.Domain.Awards.Models;

/// <summary>
/// Registro simples de auditoria para premiações
/// </summary>
public class RewardLog
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public DateTime OccurredAt { get; private set; }

    protected RewardLog() { }

    public RewardLog(Guid id, Guid userId, string action)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id não pode ser vazio.", nameof(id));
        if (userId == Guid.Empty) throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));
    if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("Ação é obrigatória.", nameof(action));

    Id = id;
    UserId = userId;
    Action = action;
    OccurredAt = DateTime.UtcNow; // carimbo de data/hora automático (UTC)
    }
}
