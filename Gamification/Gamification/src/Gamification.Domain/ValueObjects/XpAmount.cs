using System;

namespace Gamification.Domain.ValueObjects;

/// <summary>
/// Representa uma quantidade de XP imutável e não-negativa.
/// </summary>
public sealed class XpAmount : IEquatable<XpAmount>
{
    public int Value { get; }

    public XpAmount(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Quantidade de XP deve ser zero ou positiva.");

        Value = value;
    }

    public bool Equals(XpAmount? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is XpAmount other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}
