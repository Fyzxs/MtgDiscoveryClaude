using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Tests.Fakes;
using Lib.Cosmos.Apis.Operators;
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
    public async Task Constructor_WithGopherAndScribe_UsesProvidedComponents()
    {
        // Arrange
        UserCardsGopherFake gopher = new();
        UserCardsScribeFake scribe = new();

        // Use TypeWrapper to access internal constructor
        UserCardsCommandAdapter adapter = new InstanceWrapper(gopher, scribe);

        IAddUserCardXfrEntity addUserCard = new AddUserCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { new UserCardDetailsXfrEntityFake { Finish = "nonfoil", Special = "none", Count = 1 } }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        gopher.ReadAsyncCallCount.Should().Be(1);
        scribe.UpsertAsyncCallCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithValidUserCard_ReturnsSuccessResponse()
    {
        // Arrange
        UserCardsGopherFake gopher = new();
        UserCardsScribeFake scribe = new();

        // Use TypeWrapper to access internal constructor
        UserCardsCommandAdapter adapter = new InstanceWrapper(gopher, scribe);

        IUserCardDetailsXfrEntity collectedCard = new UserCardDetailsXfrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IAddUserCardXfrEntity addUserCard = new AddUserCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        actual.ResponseData.SetId.Should().Be("set789");
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WhenCosmosUpsertFails_ReturnsFailureResponse()
    {
        // Arrange
        UserCardsGopherFake gopher = new();
        UserCardsScribeFake scribe = new() { ShouldReturnFailure = true, FailureStatusCode = HttpStatusCode.InternalServerError };
        UserCardsCommandAdapter adapter = new InstanceWrapper(gopher, scribe);

        IUserCardDetailsXfrEntity collectedCard = new UserCardDetailsXfrEntityFake
        {
            Finish = "foil",
            Special = "altered",
            Count = 2
        };

        IAddUserCardXfrEntity addUserCard = new AddUserCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserCardsAdapterException>();
        gopher.ReadAsyncCallCount.Should().Be(1);
        scribe.UpsertAsyncCallCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithExistingRecord_MergesCollectedItems()
    {
        // Arrange
        UserCardExtEntity existingRecord = new()
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = [new UserCardDetailsExtEntity { Finish = "nonfoil", Special = "none", Count = 1 }]
        };

        UserCardsGopherFake gopher = new() { ShouldReturnExistingRecord = true, ExistingRecord = existingRecord };
        UserCardsScribeFake scribe = new();
        UserCardsCommandAdapter adapter = new InstanceWrapper(gopher, scribe);

        IUserCardDetailsXfrEntity newCollectedCard = new UserCardDetailsXfrEntityFake
        {
            Finish = "foil",
            Special = "none",
            Count = 2
        };

        IAddUserCardXfrEntity addUserCard = new AddUserCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { newCollectedCard }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        gopher.ReadAsyncCallCount.Should().Be(1);
        scribe.UpsertAsyncCallCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithNoExistingRecord_CreatesNewRecord()
    {
        // Arrange
        UserCardsGopherFake gopher = new() { ShouldReturnExistingRecord = false };
        UserCardsScribeFake scribe = new();
        UserCardsCommandAdapter adapter = new InstanceWrapper(gopher, scribe);

        IUserCardDetailsXfrEntity collectedCard = new UserCardDetailsXfrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 3
        };

        IAddUserCardXfrEntity addUserCard = new AddUserCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = new[] { collectedCard }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        actual.ResponseData.SetId.Should().Be("set789");
        gopher.ReadAsyncCallCount.Should().Be(1);
        scribe.UpsertAsyncCallCount.Should().Be(1);
    }
}

internal sealed class InstanceWrapper : TypeWrapper<UserCardsCommandAdapter>
{
    public InstanceWrapper(ICosmosGopher gopher, ICosmosScribe scribe) : base(gopher, scribe) { }
}
