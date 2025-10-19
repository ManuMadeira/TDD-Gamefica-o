using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Ports;

namespace Gamification.Domain.Tests.Fakes
{
    public class InMemoryReadStore : IAwardsReadStore
    {
        public HashSet<(Guid, Guid, Guid, string)> ExistingBadges = new();
        public HashSet<(Guid, Guid)> CompletedMissions = new();
        public Dictionary<Guid, Guid> RequestMap = new();
        public Dictionary<Guid, (DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)> ThemePolicies = new();

        public Task<bool> MissionCompletedAsync(Guid userId, Guid missionId, CancellationToken ct = default)
            => Task.FromResult(CompletedMissions.Contains((userId, missionId)));

        public Task<bool> BadgeExistsByNaturalKeyAsync(Guid userId, Guid themeId, Guid missionId, BadgeSlug badgeSlug, CancellationToken ct = default)
            => Task.FromResult(ExistingBadges.Contains((userId, themeId, missionId, badgeSlug.Value)));

        public Task<Guid?> GetAwardIdByRequestIdAsync(Guid requestId, CancellationToken ct = default)
            => Task.FromResult(RequestMap.TryGetValue(requestId, out var awardId) ? (Guid?)awardId : null);

        public Task<(DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)> GetThemeBonusPolicyAsync(Guid themeId, CancellationToken ct = default)
        {
            if (ThemePolicies.TryGetValue(themeId, out var p))
                return Task.FromResult(p);
            return Task.FromResult<(DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)>((null, null, null, 0, 0, 0));
        }

        // Métodos adicionais usados pelo fake UoW
        public Task<object?> GetByIdAsync(Guid id, CancellationToken ct = default) => Task.FromResult<object?>(null);
    }

    public class InMemoryWriteStore : IAwardsWriteStore
    {
        public List<BadgeAward> Badges = new();
        public List<Award> Awards = new();
        public List<RewardLog> Logs = new();
        public Dictionary<Guid, Guid> RequestMap = new();

        public Task CreateBadgeAwardAsync(BadgeAward badgeAward, Guid? requestId = null, CancellationToken ct = default)
        {
            Badges.Add(badgeAward);
            if (requestId.HasValue) RequestMap[requestId.Value] = badgeAward.Id;
            return Task.CompletedTask;
        }

        public Task CreateAsync(Award award, CancellationToken ct = default)
        {
            Awards.Add(award);
            return Task.CompletedTask;
        }

        public Task CreateAsync(RewardLog log, CancellationToken ct = default)
        {
            Logs.Add(log);
            return Task.CompletedTask;
        }
    }

    public class InMemoryUnitOfWork : IAwardsUnitOfWork
    {
        public InMemoryUnitOfWork(InMemoryReadStore read, InMemoryWriteStore write)
        {
            ReadStore = read;
            WriteStore = write;
        }

        public IAwardsReadStore ReadStore { get; }
        public IAwardsWriteStore WriteStore { get; }

        public Task BeginTransactionAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task CommitTransactionAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task RollbackTransactionAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}