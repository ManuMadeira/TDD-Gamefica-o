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
    public class AwardBadgeServiceTests
    {
        [Fact]
        public async Task ConcederBadge_missao_concluida_concede_uma_vez()
        {
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("first-mission");
            var now = DateTimeOffset.UtcNow;

            var read = new InMemoryReadStore();
            var write = new InMemoryWriteStore();
            var uow = new InMemoryUnitOfWork(read, write);

            read.CompletedMissions.Add((userId, missionId));
            read.ThemePolicies[themeId] = (now.AddDays(-1), now.AddDays(1), now.AddDays(3), 10, 20, 0.5);

            var service = new AwardBadgeService(uow);

            var result = await service.ConcederBadgeAsync(userId, themeId, missionId, slug, now);

            result.Should().NotBeNull();
            write.Badges.Should().ContainSingle();

            // Tentar conceder novamente deve falhar (unicidade)
            await Assert.ThrowsAsync<DomainException>(() =>
                service.ConcederBadgeAsync(userId, themeId, missionId, slug, now));
        }

        [Fact]
        public async Task ConcederBadge_sem_concluir_missao_falha()
        {
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("mission-x");
            var now = DateTimeOffset.UtcNow;

            var read = new InMemoryReadStore();
            var write = new InMemoryWriteStore();
            var uow = new InMemoryUnitOfWork(read, write);

            var service = new AwardBadgeService(uow);

            await Assert.ThrowsAsync<DomainException>(() =>
                service.ConcederBadgeAsync(userId, themeId, missionId, slug, now));
        }

        [Fact]
        public async Task ConcederBadge_mesma_requisicao_idempotente()
        {
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("mission-idempotent");
            var now = DateTimeOffset.UtcNow;
            var requestId = Guid.NewGuid();

            var read = new InMemoryReadStore();
            var write = new InMemoryWriteStore();
            var uow = new InMemoryUnitOfWork(read, write);

            read.CompletedMissions.Add((userId, missionId));
            read.ThemePolicies[themeId] = (now.AddDays(-1), now.AddDays(1), now.AddDays(3), 10, 20, 0.5);

            var service = new AwardBadgeService(uow);

            var first = await service.ConcederBadgeAsync(userId, themeId, missionId, slug, now, requestId);
            // Simular leitura futura do mesmo requestId
            read.RequestMap[requestId] = first.Id;

            var second = await service.ConcederBadgeAsync(userId, themeId, missionId, slug, now, requestId);

            second.Id.Should().Be(first.Id);
            write.Badges.Should().ContainSingle();
        }

        [Fact]
        public async Task ConcederBadge_chave_natural_evita_duplicacao()
        {
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("mission-y");
            var now = DateTimeOffset.UtcNow;

            var read = new InMemoryReadStore();
            var write = new InMemoryWriteStore();
            var uow = new InMemoryUnitOfWork(read, write);

            read.CompletedMissions.Add((userId, missionId));
            read.ExistingBadges.Add((userId, themeId, missionId, slug.Value));

            var service = new AwardBadgeService(uow);

            await Assert.ThrowsAsync<DomainException>(() =>
                service.ConcederBadgeAsync(userId, themeId, missionId, slug, now));
        }

        [Fact]
        public async Task ConcederBadge_missao_inexistente_falha()
        {
            var userId = Guid.NewGuid();
            var themeId = Guid.NewGuid();
            var missionId = Guid.NewGuid();
            var slug = new BadgeSlug("nonexistent");
            var now = DateTimeOffset.UtcNow;

            var read = new InMemoryReadStore();
            var write = new InMemoryWriteStore();
            var uow = new InMemoryUnitOfWork(read, write);

            var service = new AwardBadgeService(uow);

            await Assert.ThrowsAsync<DomainException>(() =>
                service.ConcederBadgeAsync(userId, themeId, missionId, slug, now));
        }
    }
}