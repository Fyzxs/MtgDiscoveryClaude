using System;
using Lib.Adapter.Scryfall.Cosmos.Cosmos.Containers;
using Microsoft.Extensions.Logging;

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

internal sealed class LoggerFake : ILogger
{
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
}
