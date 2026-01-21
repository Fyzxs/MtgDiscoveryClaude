using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Tests.Fakes;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.Cards.Tests.Apis;

[TestClass]
public sealed class CardAdapterServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        CardQueryAdapterFake queryAdapterFake = new();

        // Act
        CardAdapterService subject = new InstanceWrapper(queryAdapterFake);

        // Assert
        subject.Should().BeAssignableTo<ICardAdapterService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCardsByIdsAsync_WithValidInput_DelegatesToQueryAdapter()
    {
        // Arrange
        IEnumerable<ScryfallCardItemExtEntity> expectedResults =
        [
            new() { Data = new { id = "card1", name = "Test Card 1" } },
            new() { Data = new { id = "card2", name = "Test Card 2" } }
        ];

        IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> operationResponse = new OperationResponseFake<IEnumerable<ScryfallCardItemExtEntity>>
        {
            IsSuccess = true,
            ResponseData = expectedResults
        };

        CardQueryAdapterFake queryAdapterFake = new() { GetCardsByIdsAsyncResult = operationResponse };
        CardAdapterService subject = new InstanceWrapper(queryAdapterFake);

        CardIdsItrEntityFake cardIds = new() { CardIds = new[] { "card1", "card2" } };

        // Act
        IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> actual = await subject.GetCardsByIdsAsync(cardIds).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        queryAdapterFake.GetCardsByIdsAsyncInvokeCount.Should().Be(1);
        queryAdapterFake.GetCardsBySetCodeAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.GetCardsByNameAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.SearchCardNamesAsyncInvokeCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCardsBySetCodeAsync_WithValidInput_DelegatesToQueryAdapter()
    {
        // Arrange
        IEnumerable<ScryfallSetCardItemExtEntity> expectedResults =
        [
            new() { Data = new { id = "card1", set_id = "set1" } },
            new() { Data = new { id = "card2", set_id = "set1" } }
        ];

        IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> operationResponse = new OperationResponseFake<IEnumerable<ScryfallSetCardItemExtEntity>>
        {
            IsSuccess = true,
            ResponseData = expectedResults
        };

        CardQueryAdapterFake queryAdapterFake = new() { GetCardsBySetCodeAsyncResult = operationResponse };
        CardAdapterService subject = new InstanceWrapper(queryAdapterFake);

        SetCodeItrEntityFake setCode = new() { SetCode = "set1" };

        // Act
        IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> actual = await subject.GetCardsBySetCodeAsync(setCode).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        queryAdapterFake.GetCardsBySetCodeAsyncInvokeCount.Should().Be(1);
        queryAdapterFake.GetCardsByIdsAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.GetCardsByNameAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.SearchCardNamesAsyncInvokeCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCardsByNameAsync_WithValidInput_DelegatesToQueryAdapter()
    {
        // Arrange
        IEnumerable<ScryfallCardByNameExtEntity> expectedResults =
        [
            new() { NameGuid = "guid1", Data = new { id = "card1", name = "Lightning Bolt" } }
        ];

        IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> operationResponse = new OperationResponseFake<IEnumerable<ScryfallCardByNameExtEntity>>
        {
            IsSuccess = true,
            ResponseData = expectedResults
        };

        CardQueryAdapterFake queryAdapterFake = new() { GetCardsByNameAsyncResult = operationResponse };
        CardAdapterService subject = new InstanceWrapper(queryAdapterFake);

        CardNameItrEntityFake cardName = new() { CardName = "Lightning Bolt" };

        // Act
        IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> actual = await subject.GetCardsByNameAsync(cardName).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        queryAdapterFake.GetCardsByNameAsyncInvokeCount.Should().Be(1);
        queryAdapterFake.GetCardsByIdsAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.GetCardsBySetCodeAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.SearchCardNamesAsyncInvokeCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task SearchCardNamesAsync_WithValidInput_DelegatesToQueryAdapter()
    {
        // Arrange
        IEnumerable<string> expectedResults =
        [
            "Lightning Bolt",
            "Lightning Strike"
        ];

        IOperationResponse<IEnumerable<string>> operationResponse = new OperationResponseFake<IEnumerable<string>>
        {
            IsSuccess = true,
            ResponseData = expectedResults
        };

        CardQueryAdapterFake queryAdapterFake = new() { SearchCardNamesAsyncResult = operationResponse };
        CardAdapterService subject = new InstanceWrapper(queryAdapterFake);

        CardSearchTermItrEntityFake searchTerm = new() { SearchTerm = "Lightning" };

        // Act
        IOperationResponse<IEnumerable<string>> actual = await subject.SearchCardNamesAsync(searchTerm).ConfigureAwait(false);

        // Assert
        actual.Should().Be(operationResponse);
        queryAdapterFake.SearchCardNamesAsyncInvokeCount.Should().Be(1);
        queryAdapterFake.GetCardsByIdsAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.GetCardsBySetCodeAsyncInvokeCount.Should().Be(0);
        queryAdapterFake.GetCardsByNameAsyncInvokeCount.Should().Be(0);
    }

    private sealed class InstanceWrapper : TypeWrapper<CardAdapterService>
    {
        public InstanceWrapper(ICardQueryAdapter queryAdapter)
            : base(queryAdapter) { }
    }
}
