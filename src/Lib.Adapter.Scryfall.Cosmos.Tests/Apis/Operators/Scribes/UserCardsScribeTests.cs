using System;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

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

internal sealed class LoggerFake : ILogger
{
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
}
