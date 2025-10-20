using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Ports;

namespace Gamification.Domain.Tests.Fakes
{
    public class FakeAwardsReadStore : IAwardsReadStore
    {
        public HashSet<(Guid, Guid, Guid, string)> ExistingBadges { get; } = new();
        public HashSet<(Guid, Guid)> CompletedMissions { get; } = new();
        public Dictionary<Guid, Guid> RequestMap { get; } = new();
        public Dictionary<Guid, (DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)> ThemePolicies { get; } = new();

        public Task<bool> BadgeExistsAsync(Guid userId, Guid themeId, Guid missionId, string badgeSlug, CancellationToken ct = default)
            => Task.FromResult(ExistingBadges.Contains((userId, themeId, missionId, badgeSlug)));

        public Task<bool> HasCompletedMissionAsync(Guid userId, Guid missionId, CancellationToken ct = default)
            => Task.FromResult(CompletedMissions.Contains((userId, missionId)));

        public Task<Guid?> FindRequestByUserAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult(RequestMap.ContainsKey(userId) ? (Guid?)RequestMap[userId] : null);

        public Task<(DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)?> GetThemePolicyAsync(Guid themeId, CancellationToken ct = default)
            => Task.FromResult(ThemePolicies.ContainsKey(themeId) ? ((DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)?)ThemePolicies[themeId] : null);

        // Other read methods from IAwardsReadStore - simple defaults to satisfy interface
        public Task<Award?> GetByIdAsync(Guid awardId, CancellationToken cancellationToken = default) => Task.FromResult<Award?>(null);
        public Task<bool> ExistsAsync(Guid awardId, CancellationToken cancellationToken = default) => Task.FromResult(false);
        public Task<Guid?> GetAwardIdByRequestIdAsync(Guid requestId, CancellationToken ct = default) => Task.FromResult<Guid?>(null);
        public Task<bool> MissionCompletedAsync(Guid userId, Guid missionId, CancellationToken cancellationToken = default) => Task.FromResult(false);
        public Task<(DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)?> GetThemeBonusPolicyAsync(Guid themeId, CancellationToken ct = default) => Task.FromResult<(DateTimeOffset?, DateTimeOffset?, DateTimeOffset?, int, int, double)?>(null);
    }
}
