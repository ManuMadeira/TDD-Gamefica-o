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
        }

        [Fact]
        public void BonusPolicy_ate_fullWeight_concede_integral()
        {
            // Arrange
            var policy = new BonusPolicy();
            var now = new DateTime(2025, 10, 20);
            var bonusFullWeightEndDate = new DateTime(2025, 10, 31);
            var finalDate = new DateTime(2025, 11, 15);
            var totalXp = 1000;

            // Act
            var result = policy.CalcularBonus(now, bonusFullWeightEndDate, finalDate, totalXp);

            // Assert
            result.Xp.Should().Be(totalXp);
            result.Justification.Should().Be("Bônus integral concedido.");
        }
        [Fact]
        public void BonusPolicy_entre_datas_concede_reduzido()
        {
            // Arrange
            var policy = new BonusPolicy();
            var now = new DateTime(2025, 11, 5);
            var bonusFullWeightEndDate = new DateTime(2025, 10, 31);
            var finalDate = new DateTime(2025, 11, 15);
            var totalXp = 1000;
            var expectedReducedXp = 500;

            // Act
            var result = policy.CalcularBonus(now, bonusFullWeightEndDate, finalDate, totalXp);

            // Assert
            result.Xp.Should().Be(expectedReducedXp);
            result.Justification.Should().Be("Bônus reduzido concedido.");
        }
        [Fact]
        public void BonusPolicy_apos_finalDate_sem_bonus()
        {
            // Arrange
            var policy = new BonusPolicy();
            var now = new DateTime(2025, 11, 20);
            var bonusFullWeightEndDate = new DateTime(2025, 10, 31);
            var finalDate = new DateTime(2025, 11, 15);
            var totalXp = 1000;

            // Act
            var result = policy.CalcularBonus(now, bonusFullWeightEndDate, finalDate, totalXp);

            // Assert
            result.Xp.Should().Be(0);
            result.Justification.Should().Be("Nenhum bônus aplicável.");
        }
        [Fact]
        public void BonusPolicy_datas_inconsistentes_lanca_excecao()
        {
            // Arrange
            var policy = new BonusPolicy();
            var now = new DateTime(2025, 10, 20);
            var bonusFullWeightEndDate = new DateTime(2025, 11, 15);
            var finalDate = new DateTime(2025, 10, 31);
            var totalXp = 1000;

            // Act
            Action act = () => policy.CalcularBonus(now, bonusFullWeightEndDate, finalDate, totalXp);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("A data final do bônus integral não pode ser posterior à data final geral.");
        }
        [Fact]
        public void BonusPolicy_timezone_usa_UTC_para_comparacao()
        {
            // Arrange
            var policy = new BonusPolicy();

            var now = new DateTimeOffset(2025, 10, 31, 22, 0, 0, TimeSpan.FromHours(-3));

            var bonusFullWeightEndDate = new DateTimeOffset(2025, 10, 31, 0, 0, 0, TimeSpan.Zero);

            var finalDate = new DateTimeOffset(2025, 11, 15, 0, 0, 0, TimeSpan.Zero);
            var totalXp = 1000;

            // Act
            var result = policy.CalcularBonus(now, bonusFullWeightEndDate, finalDate, totalXp);

            // Assert
            result.Xp.Should().Be(totalXp / 2);
            result.Justification.Should().Be("Bônus reduzido concedido.");
        }
    }
}