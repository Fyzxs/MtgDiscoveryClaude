using System.Threading.Tasks;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Tests.Fakes;
using Lib.Cosmos.Apis.Operators;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
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
        _ = new LoggerFake();
        UserCardsScribeFake scribe = new();

        // Use TypeWrapper to access internal constructor
        UserCardsCommandAdapter adapter = new InstanceWrapper(scribe);

        IUserCardItrEntity userCard = new UserCardItrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { new UserCardDetailsItrEntityFake { Finish = "nonfoil", Special = "none", Count = 1 } }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

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

        IUserCardDetailsItrEntity collectedCard = new UserCardDetailsItrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IUserCardItrEntity userCard = new UserCardItrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

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

        IUserCardDetailsItrEntity collectedCard = new UserCardDetailsItrEntityFake
        {
            Finish = "foil",
            Special = "altered",
            Count = 2
        };

        IUserCardItrEntity userCard = new UserCardItrEntityFake
        {
            UserId = "failuser",
            CardId = "failcard",
            SetId = "failset",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

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

        IUserCardItrEntity userCard = new UserCardItrEntityFake
        {
            UserId = null, // This should return failure response
            CardId = "card456",
            SetId = "set789",
            CollectedList = []
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(userCard).ConfigureAwait(false);

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
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(null).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserCardsAdapterException>();
        actual.OuterException.Message.Should().Contain("User card cannot be null");
    }
}

internal sealed class InstanceWrapper : TypeWrapper<UserCardsCommandAdapter>
{
    public InstanceWrapper(ICosmosScribe scribe) : base(scribe, null, null) { }
}
