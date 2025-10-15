using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using TestConvenience.Core.Fakes;

namespace Lib.Adapter.Scryfall.Cosmos.Tests.Apis.Operators.Scribes;

[TestClass]
public sealed class UserCardsScribeTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsScribe actual = new(logger);

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_InheritsFromCosmosScribe()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsScribe actual = new(logger);

        // Assert
        actual.Should().BeAssignableTo<CosmosScribe>();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsICosmosScribe()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsScribe actual = new(logger);

        // Assert
        actual.Should().BeAssignableTo<ICosmosScribe>();
    }
}

