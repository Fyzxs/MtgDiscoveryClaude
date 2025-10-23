using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using TestConvenience.Core.Fakes;

namespace Lib.Adapter.Scryfall.Cosmos.Tests.Cosmos.Containers;

[TestClass]
public sealed class UserCardsCosmosContainerTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsCosmosContainer actual = new(logger);

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_InheritsFromCosmosContainerAdapter()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsCosmosContainer actual = new(logger);

        // Assert
        actual.Should().BeAssignableTo<Lib.Cosmos.Apis.CosmosContainerAdapter>();
    }

}

