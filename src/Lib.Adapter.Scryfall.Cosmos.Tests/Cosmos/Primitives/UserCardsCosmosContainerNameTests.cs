using Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;

namespace Lib.Adapter.Scryfall.Cosmos.Tests.Cosmos.Primitives;

[TestClass]
public sealed class UserCardsCosmosContainerNameTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_CreatesInstance()
    {
        // Arrange & Act
        UserCardsCosmosContainerName actual = new();

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ReturnsExpectedValue()
    {
        // Arrange
        UserCardsCosmosContainerName containerName = new();

        // Act
        string actual = containerName.AsSystemType();

        // Assert
        actual.Should().Be("UserCards");
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_IsConsistentAcrossInstances()
    {
        // Arrange
        UserCardsCosmosContainerName first = new();
        UserCardsCosmosContainerName second = new();

        // Act
        string firstResult = first.AsSystemType();
        string secondResult = second.AsSystemType();

        // Assert
        firstResult.Should().Be(secondResult);
        firstResult.Should().Be("UserCards");
    }
}
