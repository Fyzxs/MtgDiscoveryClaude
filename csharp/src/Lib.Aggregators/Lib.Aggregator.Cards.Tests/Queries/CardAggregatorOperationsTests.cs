using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Cards.Tests.Queries;

[TestClass]
public sealed class CardAggregatorOperationsTests
{
    private sealed class TestableCardsQueryAggregator : TypeWrapper<CardsQueryAggregator>
    {
        public TestableCardsQueryAggregator(
            ICardsByIdsAggregatorService cardsByIdsOperations,
            ICardsBySetCodeAggregatorService cardsBySetCodeOperations,
            ICardsByNameAggregatorService cardsByNameOperations,
            ICardNameSearchAggregatorService cardNameSearchOperations)
            : base(cardsByIdsOperations, cardsBySetCodeOperations, cardsByNameOperations, cardNameSearchOperations)
        { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        LoggerFake logger = new();

        // Act
        CardsQueryAggregator _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithEmptyIds_ReturnsEmptyCollection()
    {
        // Arrange
        CardIdsItrEntityFake args = new() { CardIds = [] };
        CardsByIdsAggregatorServiceFake fakeCardsByIdsService = new()
        {
            ExecuteResult = new SuccessOperationResponse<ICardItemCollectionOufEntity>(
                new CardItemCollectionOufEntityFake { Data = [] })
        };
        CardsBySetCodeAggregatorServiceFake fakeCardsBySetCodeService = new();
        CardsByNameAggregatorServiceFake fakeCardsByNameService = new();
        CardNameSearchAggregatorServiceFake fakeCardNameSearchService = new();

        CardsQueryAggregator subject = new TestableCardsQueryAggregator(
            fakeCardsByIdsService, fakeCardsBySetCodeService, fakeCardsByNameService, fakeCardNameSearchService);

        // Act
        IOperationResponse<ICardItemCollectionOufEntity> actual =
            await subject.CardsByIdsAsync(args, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Data.Should().BeEmpty();
        fakeCardsByIdsService.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithSingleId_ReturnsMatchingCard()
    {
        // Arrange
        const string cardId = "test-card-id";

        CardItemOufEntityFake expectedCard = new() { Id = cardId };
        List<ICardItemOufEntity> cardResults = [expectedCard];

        CardIdsItrEntityFake args = new() { CardIds = [cardId] };
        CardsByIdsAggregatorServiceFake fakeCardsByIdsService = new()
        {
            ExecuteResult = new SuccessOperationResponse<ICardItemCollectionOufEntity>(
                new CardItemCollectionOufEntityFake { Data = cardResults })
        };
        CardsBySetCodeAggregatorServiceFake fakeCardsBySetCodeService = new();
        CardsByNameAggregatorServiceFake fakeCardsByNameService = new();
        CardNameSearchAggregatorServiceFake fakeCardNameSearchService = new();

        CardsQueryAggregator subject = new TestableCardsQueryAggregator(
            fakeCardsByIdsService, fakeCardsBySetCodeService, fakeCardsByNameService, fakeCardNameSearchService);

        // Act
        IOperationResponse<ICardItemCollectionOufEntity> actual =
            await subject.CardsByIdsAsync(args, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Data.Should().HaveCount(1);
        actual.ResponseData.Data.First().Id.Should().Be(cardId);
        fakeCardsByIdsService.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithServiceFailure_ReturnsFailure()
    {
        // Arrange
        CardIdsItrEntityFake args = new() { CardIds = ["card1"] };
        CardsByIdsAggregatorServiceFake fakeCardsByIdsService = new()
        {
            ExecuteResult = new FailureOperationResponse<ICardItemCollectionOufEntity>(
                new CardAggregatorOperationException("Service failed"))
        };
        CardsBySetCodeAggregatorServiceFake fakeCardsBySetCodeService = new();
        CardsByNameAggregatorServiceFake fakeCardsByNameService = new();
        CardNameSearchAggregatorServiceFake fakeCardNameSearchService = new();

        CardsQueryAggregator subject = new TestableCardsQueryAggregator(
            fakeCardsByIdsService, fakeCardsBySetCodeService, fakeCardsByNameService, fakeCardNameSearchService);

        // Act
        IOperationResponse<ICardItemCollectionOufEntity> actual =
            await subject.CardsByIdsAsync(args, CancellationToken.None).ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
        fakeCardsByIdsService.ExecuteInvokeCount.Should().Be(1);
    }
}
