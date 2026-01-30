using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Tests.Fakes;
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
    public async Task Constructor_WithAddUserCardAdapter_UsesProvidedComponent()
    {
        // Arrange
        AddUserCardAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserCardExtEntity>(new UserCardExtEntity
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                CollectedList = []
            })
        };

        UserCardsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

        IAddUserCardXfrEntity addUserCard = new AddUserCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            Details = new UserCardDetailsXfrEntityFake { Finish = "nonfoil", Special = "none", Count = 1 }
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithValidUserCard_ReturnsSuccessResponse()
    {
        // Arrange
        AddUserCardAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserCardExtEntity>(new UserCardExtEntity
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                CollectedList = [new UserCardDetailsExtEntity { Finish = "nonfoil", Special = "none", Count = 1 }]
            })
        };

        UserCardsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

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
            Details = collectedCard
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
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WhenAdapterFails_ReturnsFailureResponse()
    {
        // Arrange
        AddUserCardAdapterFake fakeAdapter = new() { ShouldReturnFailure = true };
        UserCardsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

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
            Details = collectedCard
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserCardsAdapterException>();
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithExistingRecord_MergesCollectedItems()
    {
        // Arrange
        AddUserCardAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserCardExtEntity>(new UserCardExtEntity
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                CollectedList =
                [
                    new UserCardDetailsExtEntity { Finish = "nonfoil", Special = "none", Count = 1 },
                    new UserCardDetailsExtEntity { Finish = "foil", Special = "none", Count = 2 }
                ]
            })
        };

        UserCardsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

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
            Details = newCollectedCard
        };

        // Act
        IOperationResponse<UserCardExtEntity> actual = await adapter.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithNoExistingRecord_CreatesNewRecord()
    {
        // Arrange
        AddUserCardAdapterFake fakeAdapter = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserCardExtEntity>(new UserCardExtEntity
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                CollectedList = [new UserCardDetailsExtEntity { Finish = "nonfoil", Special = "none", Count = 3 }]
            })
        };

        UserCardsCommandAdapter adapter = new InstanceWrapper(fakeAdapter);

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
            Details = collectedCard
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
        fakeAdapter.ExecuteInvokeCount.Should().Be(1);
    }
}

internal sealed class InstanceWrapper : TypeWrapper<UserCardsCommandAdapter>
{
    public InstanceWrapper(IAddUserCardAdapter addUserCardAdapter) : base(addUserCardAdapter) { }
}
