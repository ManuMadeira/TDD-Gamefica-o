using System;
using System.Threading;
using System.Threading.Tasks;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Policies;
using Gamification.Domain.Awards.Ports;
using Gamification.Domain.Exceptions;
using Gamification.Domain.Awards.Constants;

namespace Gamification.Domain.Awards
{
    /// <summary>
    /// Serviço de concessão de badges — agregado raiz que aplica regras de negócio:
    /// elegibilidade, unicidade, idempotência e integração com BonusPolicy.
    /// Implementa atomicidade via IAwardsUnitOfWork (Begin/Commit/Rollback).
    /// </summary>
    public class AwardBadgeService
    {
        private readonly IAwardsUnitOfWork _uow;

        public AwardBadgeService(IAwardsUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        /// <summary>
        /// Concede um badge para um usuário após validações.
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
            if (userId == Guid.Empty) throw new ArgumentException(AwardMessages.ErrorInvalidUserId, nameof(userId));
            if (themeId == Guid.Empty) throw new ArgumentException(AwardMessages.ErrorInvalidThemeId, nameof(themeId));
            if (missionId == Guid.Empty) throw new ArgumentException(AwardMessages.ErrorInvalidMissionId, nameof(missionId));
            if (badgeSlug == null) throw new ArgumentNullException(nameof(badgeSlug));

            // Idempotência: se já existe award para esse requestId, retornar imediatamente
            if (requestId.HasValue)
            {
                var existingAwardId = await _uow.ReadStore.FindRequestByUserAsync(requestId.Value, ct);
                if (existingAwardId.HasValue)
                {
                    var existing = await _uow.ReadStore.GetByIdAsync(existingAwardId.Value, ct);
                    if (existing is BadgeAward ba) return ba;
                }
            }

            // Verificar pré-condições: missão concluída
            var missionCompleted = await _uow.ReadStore.HasCompletedMissionAsync(userId, missionId, ct);
            if (!missionCompleted) throw new DomainException(AwardMessages.ErrorUserNotCompletedMission);

            // Checar unicidade: se badge já existe por chave natural
            var already = await _uow.ReadStore.BadgeExistsAsync(userId, themeId, missionId, badgeSlug.Value, ct);
            if (already) throw new DomainException(AwardMessages.ErrorBadgeAlreadyGranted);

            // Recuperar política do tema e calcular bônus
            var policy = await _uow.ReadStore.GetThemePolicyAsync(themeId, ct);
            XpAmount xp = new XpAmount(0);
            string reason = string.Empty;
            if (policy.HasValue)
            {
                var (bonusStart, bonusFullWeightEnd, bonusFinalDate, xpBase, xpFullWeight, xpReducedWeight) = policy.Value;
                var bonusResult = BonusPolicy.Calculate(now, bonusStart, bonusFullWeightEnd, bonusFinalDate, xpBase, xpFullWeight, xpReducedWeight);
                xp = bonusResult.Xp;
                reason = bonusResult.Justification;
            }

            // Construir entidades
            var badgeAward = new BadgeAward(Guid.NewGuid(), userId, badgeSlug, xp);
            var log = new RewardLog(Guid.NewGuid(), userId, $"badge_awarded:{badgeSlug.Value}", source: AwardMessages.SourceMissionCompletion, reason: reason);

            // Persistir de forma atômica via método dedicado
            await SalvarConcessaoAtomicamente(badgeAward, log, requestId, ct);

            return badgeAward;
        }

        private async Task SalvarConcessaoAtomicamente(BadgeAward badgeAward, RewardLog log, Guid? requestId, CancellationToken ct)
        {
            await _uow.BeginTransactionAsync(ct);
            try
            {
                await _uow.WriteStore.CreateBadgeAwardAsync(badgeAward, requestId, ct);

                try
                {
                    await _uow.WriteStore.CreateAsync(log, ct);
                }
                catch
                {
                    // se o store não suporta logs, ignora
                }

                await _uow.CommitTransactionAsync(ct);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
