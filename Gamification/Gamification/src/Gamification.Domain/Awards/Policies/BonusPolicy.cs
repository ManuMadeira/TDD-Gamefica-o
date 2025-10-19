using System;
using Gamification.Domain.Awards.Models;

namespace Gamification.Domain.Awards.Policies
{
    public record BonusResult(XpAmount Xp, string Justification);

    public static class BonusPolicy
    {
        public static BonusResult Calculate(
            DateTimeOffset now,
            DateTimeOffset? bonusStart,
            DateTimeOffset? bonusFullWeightEnd,
            DateTimeOffset? bonusFinalDate,
            int xpBase,
            int xpFullWeight,
            double xpReducedWeight)
        {
            if (bonusStart == null || bonusFullWeightEnd == null || bonusFinalDate == null)
                return new BonusResult(new XpAmount(0), "no-bonus-config");

            if (bonusStart > bonusFullWeightEnd || bonusFullWeightEnd > bonusFinalDate)
                throw new ArgumentException("Configuração de janelas de bônus inconsistente");

            if (now <= bonusFullWeightEnd)
                return new BonusResult(new XpAmount(xpFullWeight), "bonus-full");

            if (now > bonusFullWeightEnd && now <= bonusFinalDate)
            {
                var reduced = (int)Math.Round(xpBase * xpReducedWeight);
                return new BonusResult(new XpAmount(reduced), "bonus-reduced");
            }

            return new BonusResult(new XpAmount(0), "no-bonus");
        }
    }
}