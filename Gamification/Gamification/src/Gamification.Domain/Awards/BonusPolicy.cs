using System;

namespace Gamification.Domain.Awards;

public class BonusPolicy
{
    public BonusCalculationResult CalcularBonus(
        DateTimeOffset now,
        DateTimeOffset bonusFullWeightEndDate,
        DateTimeOffset finalDate,
        int totalXp)
        
    {
        if (bonusFullWeightEndDate > finalDate)
        {
            throw new ArgumentException("A data final do bônus integral não pode ser posterior à data final geral.");
        }

        if (now <= bonusFullWeightEndDate)
        {
            return new BonusCalculationResult(totalXp, "Bônus integral concedido.");
        }

        if (now > bonusFullWeightEndDate && now <= finalDate)
        {
            var reducedXp = totalXp / 2;
            return new BonusCalculationResult(reducedXp, "Bônus reduzido concedido.");
        }

        return new BonusCalculationResult(0, "Nenhum bônus aplicável.");
    }
}