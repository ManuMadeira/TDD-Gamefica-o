namespace Gamification.Domain.Awards.Models;

/// <summary>
/// Representa uma premiação concedida a um usuário no sistema de gamificação
/// </summary>
public class Award
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int Points { get; private set; }
    public DateTime AwardedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    protected Award() { }

    public Award(Guid id, Guid userId, string type, string description, int points, DateTime? expiresAt = null)
    {
        Id = id;
        UserId = userId;
        Type = type;
        Description = description;
        Points = points;
        AwardedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    public bool IsExpired() => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
}
