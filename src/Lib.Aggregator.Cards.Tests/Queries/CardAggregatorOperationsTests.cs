using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Cards.Tests.Queries;

[TestClass]
public sealed class CardAggregatorOperationsTests
{
    private sealed class TestableCardAggregatorOperations : TypeWrapper<QueryCardAggregatorService>
    {
        public TestableCardAggregatorOperations(
            ICardAdapterService cardAdapterService,
            ICollectionCardItemExtToItrMapper cardItemMapper,
            ICollectionSetCardItemExtToItrMapper setCardItemMapper,
            ICollectionCardByNameExtToItrMapper cardByNameMapper)
            : base(cardAdapterService, cardItemMapper, setCardItemMapper, cardByNameMapper) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        LoggerFake logger = new();

        // Act
        QueryCardAggregatorService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithEmptyIds_ReturnsEmptyCollection()
    {
        // Arrange
        CardIdsItrEntityFake args = new() { CardIds = [] };
        CardAdapterServiceFake fakeAdapterService = new();
        CollectionCardItemExtToItrMapperFake fakeCardItemMapper = new();
        CollectionSetCardItemExtToItrMapperFake fakeSetCardItemMapper = new();
        CollectionCardByNameExtToItrMapperFake fakeCardByNameMapper = new();

        QueryCardAggregatorService subject = new TestableCardAggregatorOperations(
            fakeAdapterService, fakeCardItemMapper, fakeSetCardItemMapper, fakeCardByNameMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual =
            await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Data.Should().BeEmpty();
        fakeAdapterService.GetCardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardItemMapper.MapInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithSingleId_ReturnsMatchingCard()
    {
        // Arrange
        const string cardId = "test-card-id";

        CardItemItrEntityFake expectedCard = new() { Id = cardId };
        List<ScryfallCardItemExtEntity> adapterResults = [new ScryfallCardItemExtEntity()];
        List<ICardItemItrEntity> mapperResults = [expectedCard];

        CardIdsItrEntityFake args = new() { CardIds = [cardId] };
        CardAdapterServiceFake fakeAdapterService = new()
        {
            GetCardsByIdsAsyncResult = new SuccessOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>(adapterResults)
        };
        CollectionCardItemExtToItrMapperFake fakeCardItemMapper = new()
        {
            MapResult = mapperResults
        };
        CollectionSetCardItemExtToItrMapperFake fakeSetCardItemMapper = new();
        CollectionCardByNameExtToItrMapperFake fakeCardByNameMapper = new();

        QueryCardAggregatorService subject = new TestableCardAggregatorOperations(
            fakeAdapterService, fakeCardItemMapper, fakeSetCardItemMapper, fakeCardByNameMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual =
            await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Data.Should().HaveCount(1);
        actual.ResponseData.Data.First().Id.Should().Be(cardId);
        fakeAdapterService.GetCardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardItemMapper.MapInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithAdapterFailure_ReturnsFailure()
    {
        // Arrange
        CardIdsItrEntityFake args = new() { CardIds = ["card1"] };
        CardAdapterServiceFake fakeAdapterService = new()
        {
            GetCardsByIdsAsyncResult = new FailureOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>(new CardAggregatorOperationException("Adapter failed"))
        };
        CollectionCardItemExtToItrMapperFake fakeCardItemMapper = new();
        CollectionSetCardItemExtToItrMapperFake fakeSetCardItemMapper = new();
        CollectionCardByNameExtToItrMapperFake fakeCardByNameMapper = new();

        QueryCardAggregatorService subject = new TestableCardAggregatorOperations(
            fakeAdapterService, fakeCardItemMapper, fakeSetCardItemMapper, fakeCardByNameMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual =
            await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
        fakeAdapterService.GetCardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardItemMapper.MapInvokeCount.Should().Be(0);
    }

}
