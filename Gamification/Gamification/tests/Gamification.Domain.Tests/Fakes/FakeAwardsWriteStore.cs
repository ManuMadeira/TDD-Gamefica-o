using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Ports;

namespace Gamification.Domain.Tests.Fakes
{
    public class FakeAwardsWriteStore : IAwardsWriteStore
    {
        public List<Award> CreatedAwards { get; } = new();
        public List<BadgeAward> CreatedBadges { get; } = new();
        public List<RewardLog> Logs { get; } = new();

        public bool FailOnCreate { get; set; } = false;

        public Task CreateAsync(Award award, CancellationToken cancellationToken = default)
        {
            if (FailOnCreate) throw new InvalidOperationException("Simulated failure");
            CreatedAwards.Add(award);
            return Task.CompletedTask;
        }

        public Task CreateAsync(RewardLog log, CancellationToken ct = default)
        {
            if (FailOnCreate) throw new InvalidOperationException("Simulated failure");
            Logs.Add(log);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Award award, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid awardId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task CreateBadgeAwardAsync(BadgeAward badgeAward, Guid? requestId = null, CancellationToken ct = default)
        {
            if (FailOnCreate) throw new InvalidOperationException("Simulated failure");
            CreatedBadges.Add(badgeAward);
            if (requestId.HasValue) RequestMap[requestId.Value] = badgeAward.Id;
            return Task.CompletedTask;
        }

        // support structures used in tests
        public Dictionary<Guid, Guid> RequestMap { get; } = new();
    }
}
