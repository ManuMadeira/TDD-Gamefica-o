using FluentAssertions;
using Gamification.Domain.Awards.Models;
using Gamification.Domain.Awards.Ports;
using Moq;

namespace Gamification.Domain.Tests.Awards.Ports;

public class PortsTests
{
    private readonly Mock<IAwardsReadStore> _readStoreMock;
    private readonly Mock<IAwardsWriteStore> _writeStoreMock;
    private readonly Mock<IAwardsUnitOfWork> _unitOfWorkMock;

    public PortsTests()
    {
        _readStoreMock = new Mock<IAwardsReadStore>();
        _writeStoreMock = new Mock<IAwardsWriteStore>();
        _unitOfWorkMock = new Mock<IAwardsUnitOfWork>();
    }

    [Fact]
    public async Task AwardsReadStore_GetByIdAsync_ShouldReturnAward()
    {
        // Arrange
        var awardId = Guid.NewGuid();
        var expectedAward = new Award(awardId, Guid.NewGuid(), "Badge", "Test", 100);
        
        _readStoreMock
            .Setup(x => x.GetByIdAsync(awardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAward);

        // Act
        var result = await _readStoreMock.Object.GetByIdAsync(awardId);

        // Assert
        result.Should().Be(expectedAward);
    }

    [Fact]
    public async Task AwardsWriteStore_CreateAsync_ShouldCallCreateMethod()
    {
        // Arrange
        var award = new Award(Guid.NewGuid(), Guid.NewGuid(), "Badge", "Test", 100);
        
        _writeStoreMock
            .Setup(x => x.CreateAsync(award, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _writeStoreMock.Object.CreateAsync(award);

        // Assert
        _writeStoreMock.Verify(x => x.CreateAsync(award, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void AwardsUnitOfWork_ShouldExposeStores()
    {
        // Arrange
        _unitOfWorkMock.SetupGet(x => x.ReadStore).Returns(_readStoreMock.Object);
        _unitOfWorkMock.SetupGet(x => x.WriteStore).Returns(_writeStoreMock.Object);

        // Act & Assert
        _unitOfWorkMock.Object.ReadStore.Should().NotBeNull();
        _unitOfWorkMock.Object.WriteStore.Should().NotBeNull();
    }
}