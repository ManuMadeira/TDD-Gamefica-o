using FluentAssertions;
using Gamification.Domain.Awards.Models;
using Xunit;

namespace Gamification.Domain.Tests.Awards.Models;

public class AwardTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
    var type = "Badge"; // tipo mantido
    var description = "Primeira Conquista";
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
    public void Constructor_ShouldThrow_OnEmptyId()
    {
        Action act = () => _ = new Award(Guid.Empty, Guid.NewGuid(), "Badge", "Desc", 10);
        act.Should().Throw<ArgumentException>().Where(e => e.ParamName == "id");
    }

    [Fact]
    public void Constructor_ShouldThrow_OnEmptyUserId()
    {
        Action act = () => _ = new Award(Guid.NewGuid(), Guid.Empty, "Badge", "Desc", 10);
        act.Should().Throw<ArgumentException>().Where(e => e.ParamName == "userId");
    }

    [Fact]
    public void Constructor_ShouldThrow_OnNullOrWhitespaceType()
    {
        Action act1 = () => _ = new Award(Guid.NewGuid(), Guid.NewGuid(), "", "Desc", 10);
        Action act2 = () => _ = new Award(Guid.NewGuid(), Guid.NewGuid(), " ", "Desc", 10);

        act1.Should().Throw<ArgumentException>().Where(e => e.ParamName == "type");
        act2.Should().Throw<ArgumentException>().Where(e => e.ParamName == "type");
    }

    [Fact]
    public void Constructor_ShouldThrow_OnNegativePoints()
    {
        Action act = () => _ = new Award(Guid.NewGuid(), Guid.NewGuid(), "Badge", "Desc", -5);
        act.Should().Throw<ArgumentOutOfRangeException>().Where(e => e.ParamName == "points");
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