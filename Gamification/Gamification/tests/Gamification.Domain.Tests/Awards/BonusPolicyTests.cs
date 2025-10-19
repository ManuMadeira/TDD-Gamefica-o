using System;
using FluentAssertions;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Policies;
using Xunit;

namespace Gamification.Domain.Tests.Awards
{
    public class BonusPolicyTests
    {
        [Fact]
        public void Deve_retornar_bonus_cheio_quando_dentro_do_periodo_full()
        {
            // Arrange
            var now = DateTimeOffset.UtcNow;
            var start = now.AddDays(-2);
            var fullEnd = now.AddDays(1);
            var final = now.AddDays(3);

            // Act
            var result = BonusPolicy.Calculate(now, start, fullEnd, final, xpBase: 10, xpFullWeight: 20, xpReducedWeight: 0.5);

            // Assert
            result.Xp.Value.Should().Be(20);
            result.Justification.Should().Be("bonus-full");
        }

        [Fact]
        public void Deve_retornar_bonus_reduzido_quando_apos_o_full_e_antes_do_final()
        {
            // Arrange
            var now = DateTimeOffset.UtcNow;
            var start = now.AddDays(-5);
            var fullEnd = now.AddDays(-1);
            var final = now.AddDays(2);

            // Act
            var result = BonusPolicy.Calculate(now, start, fullEnd, final, xpBase: 10, xpFullWeight: 20, xpReducedWeight: 0.5);

            // Assert
            result.Xp.Value.Should().Be(5);
            result.Justification.Should().Be("bonus-reduced");
