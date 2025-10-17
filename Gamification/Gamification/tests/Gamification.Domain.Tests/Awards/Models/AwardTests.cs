using FluentAssertions;
using Gamification.Domain.Awards.Models;

namespace Gamification.Domain.Tests.Awards.Models;

public class AwardTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var type = "Badge";
        var description = "First Achievement";
        var points = 100;

        // Act
        var award = new Award(id, userId, type, description, points);

        // Assert
        award.Id.Should().Be(id);
        award.UserId.Should().Be(userId);
        award.Type.Should().Be(type);
        award.Description.Should().Be(description);
        award.Points.Should().Be(points);
        award.AwardedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        award.ExpiresAt.Should().BeNull();
    }

    [Fact]
    public void IsExpired_WhenNoExpiration_ShouldReturnFalse()
    {
        // Arrange
        var award = new Award(Guid.NewGuid(), Guid.NewGuid(), "Badge", "Test", 100);

        // Act & Assert
        award.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenFutureExpiration_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(1);
        var award = new Award(Guid.NewGuid(), Guid.NewGuid(), "Badge", "Test", 100, expiresAt);

        // Act & Assert
        award.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenPastExpiration_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var award = new Award(Guid.NewGuid(), Guid.NewGuid(), "Badge", "Test", 100, expiresAt);

        // Act & Assert
        award.IsExpired().Should().BeTrue();
    }
}