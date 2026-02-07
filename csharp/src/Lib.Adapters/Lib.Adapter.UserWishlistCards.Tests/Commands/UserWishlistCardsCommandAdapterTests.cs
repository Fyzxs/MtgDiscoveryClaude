using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Commands;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Adapter.UserWishlistCards.Tests.Fakes;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.UserWishlistCards.Tests.Commands;

[TestClass]
public sealed class UserWishlistCardsCommandAdapterTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        UserWishlistCardsCommandAdapter actual = new(logger);

        // Assert
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserWishlistCardAsync_DelegatesToAddAdapter()
    {
        // Arrange
        AddUserWishlistCardAdapterFake addFake = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserWishlistCardExtEntity>(new UserWishlistCardExtEntity
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                WishlistItems = [new UserWishlistCardDetailsExtEntity { Finish = "nonfoil", Special = "none", Count = 1 }]
            })
        };

        RemoveUserWishlistCardAdapterFake removeFake = new();
        UserWishlistCardsCommandAdapter adapter = new InstanceWrapper(addFake, removeFake);

        IUserWishlistCardDetailsXfrEntity details = new UserWishlistCardDetailsXfrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IAddUserWishlistCardXfrEntity addUserWishlistCard = new AddUserWishlistCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CardName = "Lightning Bolt",
            SetName = "Alpha",
            SetCode = "lea",
            ArtistIds = ["artist1"],
            CardNameGuid = "guid123",
            Details = details
        };

        // Act
        IOperationResponse<UserWishlistCardExtEntity> actual = await adapter.AddUserWishlistCardAsync(addUserWishlistCard, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        actual.ResponseData.SetId.Should().Be("set789");
        addFake.ExecuteInvokeCount.Should().Be(1);
        removeFake.ExecuteInvokeCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserWishlistCardAsync_WhenAdapterFails_ReturnsFailureResponse()
    {
        // Arrange
        AddUserWishlistCardAdapterFake addFake = new() { ShouldReturnFailure = true };
        RemoveUserWishlistCardAdapterFake removeFake = new();
        UserWishlistCardsCommandAdapter adapter = new InstanceWrapper(addFake, removeFake);

        IUserWishlistCardDetailsXfrEntity details = new UserWishlistCardDetailsXfrEntityFake
        {
            Finish = "foil",
            Special = "altered",
            Count = 2
        };

        IAddUserWishlistCardXfrEntity addUserWishlistCard = new AddUserWishlistCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CardName = "Lightning Bolt",
            SetName = "Alpha",
            SetCode = "lea",
            ArtistIds = ["artist1"],
            CardNameGuid = "guid123",
            Details = details
        };

        // Act
        IOperationResponse<UserWishlistCardExtEntity> actual = await adapter.AddUserWishlistCardAsync(addUserWishlistCard, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<UserWishlistCardsAdapterException>();
        addFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task RemoveUserWishlistCardAsync_DelegatesToRemoveAdapter()
    {
        // Arrange
        AddUserWishlistCardAdapterFake addFake = new();
        RemoveUserWishlistCardAdapterFake removeFake = new()
        {
            ExecuteResult = new SuccessOperationResponse<UserWishlistCardExtEntity>(new UserWishlistCardExtEntity
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                WishlistItems = []
            })
        };

        UserWishlistCardsCommandAdapter adapter = new InstanceWrapper(addFake, removeFake);

        IUserWishlistCardDetailsXfrEntity details = new UserWishlistCardDetailsXfrEntityFake
        {
            Finish = "nonfoil",
            Special = "none",
            Count = 1
        };

        IRemoveUserWishlistCardXfrEntity removeUserWishlistCard = new RemoveUserWishlistCardXfrEntityFake
        {
            UserId = "user123",
            CardId = "card456",
            Details = details
        };

        // Act
        IOperationResponse<UserWishlistCardExtEntity> actual = await adapter.RemoveUserWishlistCardAsync(removeUserWishlistCard, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.UserId.Should().Be("user123");
        actual.ResponseData.CardId.Should().Be("card456");
        removeFake.ExecuteInvokeCount.Should().Be(1);
        addFake.ExecuteInvokeCount.Should().Be(0);
    }
}

internal sealed class InstanceWrapper : TypeWrapper<UserWishlistCardsCommandAdapter>
{
    public InstanceWrapper(IAddUserWishlistCardAdapter addUserWishlistCardAdapter, IRemoveUserWishlistCardAdapter removeUserWishlistCardAdapter) : base(addUserWishlistCardAdapter, removeUserWishlistCardAdapter) { }
}
