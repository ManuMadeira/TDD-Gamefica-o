using System;

namespace Gamification.Domain.Awards.Models;

/// <summary>
/// Registro de auditoria para premiações
/// </summary>
public class RewardLog
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public DateTime OccurredAt { get; private set; }
    public string Source { get; private set; } = string.Empty;
    public string Reason { get; private set; } = string.Empty;

    protected RewardLog() { }

    public RewardLog(Guid id, Guid userId, string action, string source = "mission_completion", string reason = "")
    {
        if (id == Guid.Empty) throw new ArgumentException("Id não pode ser vazio.", nameof(id));
        if (userId == Guid.Empty) throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));
        if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("Ação é obrigatória.", nameof(action));
        if (string.IsNullOrWhiteSpace(source)) throw new ArgumentException("Source é obrigatória.", nameof(source));

        Id = id;
        UserId = userId;
        Action = action;
        OccurredAt = DateTime.UtcNow; // carimbo de data/hora automático (UTC)
        Source = source;
        Reason = reason ?? string.Empty;
    }
}
