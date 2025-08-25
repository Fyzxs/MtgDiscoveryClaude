using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Aggregator.Cards.Queries;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Cards.Tests.Queries;

[TestClass]
public sealed class CardAggregatorOperationsTests
{
    private sealed class TestableCardAggregatorOperations : TypeWrapper<QueryCardAggregatorService>
    {
        public TestableCardAggregatorOperations(
            ICosmosGopher cardGopher,
            QueryCardsIdsToReadPointItemsMapper mapper,
            ScryfallCardItemToCardItemItrEntityMapper cardMapper)
            : base(cardGopher, mapper, cardMapper) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        QueryCardAggregatorService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithEmptyIds_ReturnsEmptyCollection()
    {
        // Arrange
        FakeCardIdsItrEntity args = new() { CardIds = [] };
        FakeCosmosGopher fakeGopher = new();
        QueryCardsIdsToReadPointItemsMapper mapper = new();
        ScryfallCardItemToCardItemItrEntityMapper cardMapper = new();

        QueryCardAggregatorService subject = new TestableCardAggregatorOperations(
            fakeGopher, mapper, cardMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual =
            await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.Data.Should().BeEmpty();
        fakeGopher.ReadAsyncInvokeCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithSingleId_ReturnsMatchingCard()
    {
        // Arrange
        const string cardId = "test-card-id";
        dynamic testData = new JObject
        {
            ["id"] = cardId,
            ["name"] = "Test Card"
        };

        ScryfallCardItem scryfallCard = FakeScryfallCardItemFactory.Create(testData);
        FakeCardIdsItrEntity args = new() { CardIds = [cardId] };
        FakeCosmosGopher fakeGopher = new()
        {
            ReadAsyncResult = new FakeOpResponse<CosmosItem>(scryfallCard, HttpStatusCode.OK)
        };
        QueryCardsIdsToReadPointItemsMapper mapper = new();
        ScryfallCardItemToCardItemItrEntityMapper cardMapper = new();

        QueryCardAggregatorService subject = new TestableCardAggregatorOperations(
            fakeGopher, mapper, cardMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual =
            await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.Data.Should().HaveCount(1);
        actual.ResponseData.Data.First().Id.Should().Be(cardId);
        fakeGopher.ReadAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithFailedRead_ExcludesFailedCard()
    {
        // Arrange
        string[] cardIds = ["card1", "card2"];
        dynamic testData = new JObject
        {
            ["id"] = "card1",
            ["name"] = "Test Card 1"
        };

        ScryfallCardItem scryfallCard = FakeScryfallCardItemFactory.Create(testData);
        FakeCardIdsItrEntity args = new() { CardIds = cardIds };

        // Create a fake gopher that returns success for first call, failure for second
        FakeMultiResponseCosmosGopher fakeGopher = new()
        {
            Responses =
            [
                new FakeOpResponse<CosmosItem>(scryfallCard, HttpStatusCode.OK),
                new FakeOpResponse<CosmosItem>(null!, HttpStatusCode.NotFound)
            ]
        };

        QueryCardsIdsToReadPointItemsMapper mapper = new();
        ScryfallCardItemToCardItemItrEntityMapper cardMapper = new();

        QueryCardAggregatorService subject = new TestableCardAggregatorOperations(
            fakeGopher, mapper, cardMapper);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual =
            await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().NotBeNull();
        actual.ResponseData.Data.Should().HaveCount(1);
        actual.ResponseData.Data.First().Id.Should().Be("card1");
        fakeGopher.ReadAsyncInvokeCount.Should().Be(2);
    }

    private sealed class LoggerFake : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => new DisposableFake();
        public bool IsEnabled(LogLevel logLevel) => false;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }

        private sealed class DisposableFake : IDisposable
        {
            public void Dispose() { }
        }
    }

    private sealed class FakeMultiResponseCosmosGopher : ICosmosGopher
    {
        public List<OpResponse<CosmosItem>> Responses { get; init; } = [];
        public int ReadAsyncInvokeCount { get; private set; }
        private int _currentResponseIndex;

        public Task<OpResponse<T>> ReadAsync<T>(ReadPointItem readPointItem)
        {
            ReadAsyncInvokeCount++;

            if (_currentResponseIndex < Responses.Count)
            {
                OpResponse<CosmosItem> response = Responses[_currentResponseIndex++];

                if (response.Value is T typedValue)
                {
                    return Task.FromResult<OpResponse<T>>(new FakeOpResponse<T>(typedValue, response.StatusCode));
                }

                return Task.FromResult<OpResponse<T>>(new FakeOpResponse<T>(default!, response.StatusCode));
            }

            return Task.FromResult<OpResponse<T>>(new FakeOpResponse<T>(default!, HttpStatusCode.NotFound));
        }
    }
}