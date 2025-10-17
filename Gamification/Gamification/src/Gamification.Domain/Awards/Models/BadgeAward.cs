using System;
using Gamification.Domain.ValueObjects;

namespace Gamification.Domain.Awards.Models;

/// <summary>
/// Representa um badge concedido a um usuário. Utiliza chave natural: (UserId, BadgeSlug)
/// </summary>
public class BadgeAward
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public BadgeSlug BadgeSlug { get; private set; } = null!;
    public XpAmount Xp { get; private set; } = null!;
    public DateTime AwardedAt { get; private set; }

    protected BadgeAward() { }

    public BadgeAward(Guid id, Guid userId, BadgeSlug badgeSlug, XpAmount xp)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id não pode ser vazio.", nameof(id));
        if (userId == Guid.Empty) throw new ArgumentException("UserId não pode ser vazio.", nameof(userId));
        BadgeSlug = badgeSlug ?? throw new ArgumentNullException(nameof(badgeSlug));
        Xp = xp ?? throw new ArgumentNullException(nameof(xp));

        Id = id;
        UserId = userId;
        AwardedAt = DateTime.UtcNow;
    }

    // Igualdade por chave natural
    public override bool Equals(object? obj)
    {
        if (obj is not BadgeAward other) return false;
        return UserId == other.UserId && BadgeSlug.Equals(other.BadgeSlug);
    }

    public override int GetHashCode() => HashCode.Combine(UserId, BadgeSlug);
}
