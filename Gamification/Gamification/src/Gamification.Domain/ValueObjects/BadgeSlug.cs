using System;
using System.Text.RegularExpressions;

namespace Gamification.Domain.ValueObjects;

/// <summary>
/// Representa um identificador (slug) formatado de um badge (value object imutável).
/// Premissas: letras minúsculas, dígitos, hífens ou underscores, sem espaços, e deve iniciar/terminar com caractere alfanumérico.
/// </summary>
public sealed class BadgeSlug : IEquatable<BadgeSlug>
{
    public string Value { get; }

    public BadgeSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("O identificador (slug) do badge não pode ser nulo ou vazio.", nameof(value));

    // permite letras minúsculas, dígitos, hífens e underscores; grupos separados por - ou _
        var pattern = "^[a-z0-9]+(?:[-_][a-z0-9]+)*$";
        if (!Regex.IsMatch(value, pattern))
            throw new ArgumentException("Formato de slug inválido. Permitido: letras minúsculas, dígitos, hífens e underscores.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;

    public bool Equals(BadgeSlug? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is BadgeSlug other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}
