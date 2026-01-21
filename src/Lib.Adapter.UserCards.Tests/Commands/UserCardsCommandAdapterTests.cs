using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.UserCards.Tests.Commands;

[TestClass]
public sealed class UserCardsCommandAdapterTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserCardsCommandAdapter actual = new(logger);

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Constructor_WithLoggerAndScribe_UsesProvidedScribe()
    {
        // Arrange
        ILogger logger = new LoggerFake();
        UserCardsScribeFake scribe = new();

        // Use TypeWrapper to access internal constructor
        UserCardsCommandAdapter adapter = new InstanceWrapper(logger, scribe);

        IUserCardCollectionItrEntity userCard = new UserCardCollectionItrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { new CollectedCardItrEntityFake { Finish = "nonfoil", Special = "none", Count = 1 } }
        };

        // Act
        IOperationResponse<IUserCardCollectionItrEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        scribe.UpsertAsyncCallCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithValidUserCard_ReturnsSuccessResponse()
    {
        // Arrange
        ILogger logger = new LoggerFake();
        UserCardsCommandAdapter adapter = new(logger);

        ICollectedCardItrEntity collectedCard = new CollectedCardItrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IUserCardCollectionItrEntity userCard = new UserCardCollectionItrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<IUserCardCollectionItrEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        actual.ResponseData.SetId.Should().Be("set789");
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WhenCosmosOperationFails_ReturnsFailureResponse()
    {
        // Arrange
        ILogger logger = new LoggerFake();
        UserCardsCommandAdapter adapter = new(logger);

        ICollectedCardItrEntity collectedCard = new CollectedCardItrEntityFake
        {
            Finish = "foil",
            Special = "altered",
            Count = 2
        };

        IUserCardCollectionItrEntity userCard = new UserCardCollectionItrEntityFake
        {
            UserId = "failuser",
            CardId = "failcard",
            SetId = "failset",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<IUserCardCollectionItrEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserCardsAdapterException>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithNullUserId_ReturnsFailureResponse()
    {
        // Arrange
        ILogger logger = new LoggerFake();
        UserCardsCommandAdapter adapter = new(logger);

        IUserCardCollectionItrEntity userCard = new UserCardCollectionItrEntityFake
        {
            UserId = null, // This should return failure response
            CardId = "card456",
            SetId = "set789",
            CollectedList = []
        };

        // Act
        IOperationResponse<IUserCardCollectionItrEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserCardsAdapterException>();
        actual.OuterException.Message.Should().Contain("UserId and CardId are required");
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithNullUserCard_ReturnsFailureResponse()
    {
        // Arrange
        ILogger logger = new LoggerFake();
        UserCardsCommandAdapter adapter = new(logger);

        // Act
        IOperationResponse<IUserCardCollectionItrEntity> actual = await adapter.AddUserCardAsync(null).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserCardsAdapterException>();
        actual.OuterException.Message.Should().Contain("User card cannot be null");
    }
}

internal sealed class LoggerFake : ILogger
{
    public System.IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception exception, System.Func<TState, System.Exception, string> formatter) { }
}

internal sealed class CollectedCardItrEntityFake : ICollectedCardItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}

internal sealed class UserCardCollectionItrEntityFake : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public IEnumerable<ICollectedCardItrEntity> CollectedList { get; init; }
}

internal sealed class UserCardsScribeFake : IUserCardsScribe
{
    public int UpsertAsyncCallCount { get; private set; }

    public async Task<OpResponse<T>> UpsertAsync<T>(T item)
    {
        UpsertAsyncCallCount++;
        await Task.CompletedTask.ConfigureAwait(false);

        // Return success response with the same item
        return new OpResponseFake<T>(item, HttpStatusCode.OK);
    }
}

internal sealed class OpResponseFake<T> : OpResponse<T>
{
    public OpResponseFake(T value, HttpStatusCode statusCode)
    {
        Value = value;
        StatusCode = statusCode;
    }

    public override T Value { get; }
    public override HttpStatusCode StatusCode { get; }
}

internal sealed class InstanceWrapper : TypeWrapper<UserCardsCommandAdapter>
{
    public InstanceWrapper(ILogger logger, IUserCardsScribe scribe) : base(logger, scribe) { }
}
