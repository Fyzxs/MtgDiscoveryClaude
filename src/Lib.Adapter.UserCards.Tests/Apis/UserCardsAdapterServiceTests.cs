using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Tests.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.UserCards.Tests.Apis;

[TestClass]
public sealed class UserCardsAdapterServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        UserCardsCommandAdapterFake commandAdapterFake = new();
        UserCardsQueryAdapterFake queryAdapterFake = new();

        // Act
        UserCardsAdapterService subject = new InstanceWrapper(commandAdapterFake, queryAdapterFake);

        // Assert
        subject.Should().BeAssignableTo<IUserCardsAdapterService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task AddUserCardAsync_WithValidUserCard_DelegatesToCommandAdapter()
    {
        // Arrange
        UserCardExtEntity expectedResult = new()
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = []
        };

        IOperationResponse<UserCardExtEntity> operationResponse = new OperationResponseFake<UserCardExtEntity> { IsSuccess = true, ResponseData = expectedResult };
        UserCardsCommandAdapterFake commandAdapterFake = new() { AddUserCardAsyncResult = operationResponse };
        UserCardsQueryAdapterFake queryAdapterFake = new();
        UserCardsAdapterService subject = new InstanceWrapper(commandAdapterFake, queryAdapterFake);

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
        IOperationResponse<UserCardExtEntity> actual = await subject.AddUserCardAsync(addUserCard).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        commandAdapterFake.AddUserCardAsyncInvokeCount.Should().Be(1);
        queryAdapterFake.UserCardsBySetAsyncInvokeCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task UserCardsBySetAsync_WithValidInput_DelegatesToQueryAdapter()
    {
        // Arrange
        IEnumerable<UserCardExtEntity> expectedResults =
        [
            new()
            {
                UserId = "user123",
                CardId = "card456",
                SetId = "set789",
                CollectedList = []
            }
        ];

        IOperationResponse<IEnumerable<UserCardExtEntity>> operationResponse = new OperationResponseFake<IEnumerable<UserCardExtEntity>> { IsSuccess = true, ResponseData = expectedResults };
        UserCardsCommandAdapterFake commandAdapterFake = new();
        UserCardsQueryAdapterFake queryAdapterFake = new() { UserCardsBySetAsyncResult = operationResponse };
        UserCardsAdapterService subject = new InstanceWrapper(commandAdapterFake, queryAdapterFake);

        IUserCardsSetXfrEntity userCardsSet = new UserCardsSetXfrEntityFake
        {
            UserId = "user123",
            SetId = "set789"
        };

        // Act
        IOperationResponse<IEnumerable<UserCardExtEntity>> actual = await subject.UserCardsBySetAsync(userCardsSet).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        queryAdapterFake.UserCardsBySetAsyncInvokeCount.Should().Be(1);
        commandAdapterFake.AddUserCardAsyncInvokeCount.Should().Be(0);
    }

    private sealed class InstanceWrapper : TypeWrapper<UserCardsAdapterService>
    {
        public InstanceWrapper(IUserCardsCommandAdapter commandAdapter, IUserCardsQueryAdapter queryAdapter)
            : base(commandAdapter, queryAdapter) { }
    }
}
