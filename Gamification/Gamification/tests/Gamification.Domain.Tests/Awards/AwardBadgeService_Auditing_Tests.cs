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
    public class AwardBadgeService_Auditing_Tests
    {
        [Fact]
        public async Task ConcederBadge_registra_auditoria_corretamente()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("test-badge");
            var now = DateTimeOffset.UtcNow;

            var read = new FakeAwardsReadStore();
            read.CompletedMissions.Add((userId, missionId));
            var write = new FakeAwardsWriteStore();
            var uow = new FakeUnitOfWork();
            var service = new AwardBadgeService(uow);

            // Act
            var result = await service.ConcederBadgeAsync(userId, themeId, missionId, slug, now);

            // Assert: an audit log was created with expected source and reason (reason may be empty if no bonus)
            write.CreatedBadges.Should().NotBeEmpty();
            write.Logs.Should().NotBeEmpty();
            write.Logs[0].Source.Should().Be("mission_completion");
        }
    }
}
