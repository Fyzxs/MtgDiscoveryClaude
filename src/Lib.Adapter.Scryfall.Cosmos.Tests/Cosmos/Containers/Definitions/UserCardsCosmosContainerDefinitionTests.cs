using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers.Definitions;
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Primitives;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Adapter.Scryfall.Cosmos.Tests.Cosmos.Containers.Definitions;

[TestClass]
public sealed class UserCardsCosmosContainerDefinitionTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_CreatesInstance()
    {
        // Arrange & Act
        UserCardsCosmosContainerDefinition actual = new();

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void FriendlyAccountName_ReturnsExpectedType()
    {
        // Arrange
        UserCardsCosmosContainerDefinition definition = new();

        // Act
        CosmosFriendlyAccountName actual = definition.FriendlyAccountName();

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<MtgDiscoveryCosmosAccountName>();
    }

    [TestMethod, TestCategory("unit")]
    public void DatabaseName_ReturnsExpectedType()
    {
        // Arrange
        UserCardsCosmosContainerDefinition definition = new();

        // Act
        CosmosDatabaseName actual = definition.DatabaseName();

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<MtgDiscoveryCosmosDatabaseName>();
    }

    [TestMethod, TestCategory("unit")]
    public void ContainerName_ReturnsExpectedType()
    {
        // Arrange
        UserCardsCosmosContainerDefinition definition = new();

        // Act
        CosmosContainerName actual = definition.ContainerName();

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<UserCardsCosmosContainerName>();
    }

    [TestMethod, TestCategory("unit")]
    public void PartitionKeyPath_ReturnsExpectedType()
    {
        // Arrange
        UserCardsCosmosContainerDefinition definition = new();

        // Act
        CosmosPartitionKeyPath actual = definition.PartitionKeyPath();

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<PartitionCosmosPartitionKeyPath>();
    }

    [TestMethod, TestCategory("unit")]
    public void ContainerName_ReturnsUserCardsSystemType()
    {
        // Arrange
        UserCardsCosmosContainerDefinition definition = new();

        // Act
        CosmosContainerName containerName = definition.ContainerName();
        string actual = containerName.AsSystemType();

        // Assert
        actual.Should().Be("UserCards");
    }
}
