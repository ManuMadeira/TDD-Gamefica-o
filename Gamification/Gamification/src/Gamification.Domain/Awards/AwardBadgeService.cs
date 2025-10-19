using System;
using System.Threading;
using System.Threading.Tasks;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Policies;
using Gamification.Domain.Awards.Ports;
using Gamification.Domain.Exceptions;

namespace Gamification.Domain.Awards
{
    /// <summary>
    /// Serviço de concessão de badges — agregado raiz que aplica regras de negócio:
    /// elegibilidade, unicidade, idempotência e integração com BonusPolicy.
    /// </summary>
    public class AwardBadgeService
    {
        private readonly IAwardsUnitOfWork _uow;

        public AwardBadgeService(IAwardsUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        /// <summary>
        /// Concede um badge a um usuário, aplicando as regras de negócio do domínio.
        /// </summary>
        public async Task<BadgeAward> ConcederBadgeAsync(
            Guid userId,
            Guid themeId,
            Guid missionId,
            BadgeSlug badgeSlug,
            DateTimeOffset now,
            Guid? requestId = null,
            CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new DomainException("UserId inválido");
            if (badgeSlug is null) throw new DomainException("BadgeSlug é obrigatório");

            // 1) Idempotência (requestId)
            if (requestId.HasValue)
            {
                var existingAwardId = await _uow.ReadStore.GetAwardIdByRequestIdAsync(requestId.Value, ct);
                if (existingAwardId.HasValue)
                {
                    var existing = await _uow.ReadStore.GetByIdAsync(existingAwardId.Value, ct);
                    if (existing is BadgeAward ba) return ba;
                }
            }

            // 2) Elegibilidade — missão concluída
            var completed = await _uow.ReadStore.MissionCompletedAsync(userId, missionId, ct);
            if (!completed)
                throw new DomainException("Elegibilidade não satisfeita: missão não concluída ou inexistente");

            // 3) Unicidade — chave natural
            var already = await _uow.ReadStore.BadgeExistsByNaturalKeyAsync(userId, themeId, missionId, badgeSlug, ct);
            if (already)
                throw new DomainException("Badge já concedida para este estudante/tema/missão (unicidade)");

            // 4) Política de bônus
            var (bonusStart, fullWeightEnd, finalDate, xpBase, xpFullWeight, xpReducedWeight) =
                await _uow.ReadStore.GetThemeBonusPolicyAsync(themeId, ct);

            var bonus = BonusPolicy.Calculate(now, bonusStart, fullWeightEnd, finalDate, xpBase, xpFullWeight, xpReducedWeight);

            var badgeAward = new BadgeAward(Guid.NewGuid(), userId, badgeSlug, bonus.Xp);
            var log = new RewardLog(Guid.NewGuid(), userId, $"mission_completion:{bonus.Justification}");

            // 5) Persistência atômica
            await _uow.BeginTransactionAsync(ct);
            try
            {
                await _uow.WriteStore.CreateBadgeAwardAsync(badgeAward, requestId, ct);

                var award = new Award(Guid.NewGuid(), userId, "Badge", $"Badge {badgeSlug.Value}", bonus.Xp.Value, null);
                await _uow.WriteStore.CreateAsync(award, ct);
                await _uow.WriteStore.CreateAsync(log, ct);

                await _uow.CommitTransactionAsync(ct);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(ct);
                throw;
            }

            return badgeAward;
        }
    }
}