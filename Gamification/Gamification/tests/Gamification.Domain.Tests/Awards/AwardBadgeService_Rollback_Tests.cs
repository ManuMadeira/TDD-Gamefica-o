using System;
using System.Threading.Tasks;
using FluentAssertions;
using Gamification.Domain.Awards;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Exceptions;
using Gamification.Domain.Tests.Fakes;
using Xunit;

namespace Gamification.Domain.Tests.Awards
{
    public class AwardBadgeService_Rollback_Tests
    {
        [Fact]
        public async Task ConcederBadge_falha_gravacao_rollback_completo()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("test-badge");
            var now = DateTimeOffset.UtcNow;

            var read = new FakeAwardsReadStore();
            read.CompletedMissions.Add((userId, missionId));
            var write = new FakeAwardsWriteStore { FailOnCreate = true };
            var uow = new FakeUnitOfWork();
            var service = new AwardBadgeService(uow);

            // Act & Assert - should throw and not leave partial state
            await Assert.ThrowsAsync<Exception>(() => service.ConcederBadgeAsync(userId, themeId, missionId, slug, now));
            uow.RollbackCalled.Should().BeTrue();
        }
    }
}
