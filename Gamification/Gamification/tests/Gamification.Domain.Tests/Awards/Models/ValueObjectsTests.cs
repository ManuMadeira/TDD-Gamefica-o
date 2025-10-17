using System;
using FluentAssertions;
using Gamification.Domain.ValueObjects;
using Gamification.Domain.Awards.Models;
using Xunit;

namespace Gamification.Domain.Tests.Awards.Models;

public class ValueObjectsTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("inválido")] // contém caractere inválido (acento)
    [InlineData("MAIUSCULO")]
    [InlineData("com espaco")]
    [InlineData("-com-hifen-no-inicio")]
    public void BadgeSlug_deve_recusar_valores_invalidos(string value)
    {
        Action act = () => _ = new BadgeSlug(value);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void XpAmount_nao_aceita_valores_negativos()
    {
        Action act = () => _ = new XpAmount(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void RewardLog_deve_registrar_timestamp_automatico()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var before = DateTime.UtcNow;
    var log = new RewardLog(id, userId, "XP concedido");
        var after = DateTime.UtcNow;

        log.OccurredAt.Should().BeOnOrAfter(before);
        log.OccurredAt.Should().BeOnOrBefore(after);
    }
}
