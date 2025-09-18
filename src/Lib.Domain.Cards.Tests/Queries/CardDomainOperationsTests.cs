using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Aggregator.Cards.Apis;
using Lib.Domain.Cards.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Domain.Cards.Tests.Queries;

[TestClass]
public sealed class CardDomainOperationsTests
{
    private sealed class TestableQueryCardDomainService : TypeWrapper<QueryCardDomainService>
    {
        public TestableQueryCardDomainService(ICardAggregatorService cardAggregatorService) : base(cardAggregatorService) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        QueryCardDomainService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithValidArgs_DelegatesToAggregatorService()
    {
        // Arrange
        CardIdsItrEntityFake args = new() { CardIds = ["id1", "id2"] };
        CardItemCollectionItrEntityFake expectedResponse = new();
        CardAggregatorServiceFake fakeAggregator = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };

        QueryCardDomainService subject = new TestableQueryCardDomainService(fakeAggregator);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().BeSameAs(expectedResponse);
        fakeAggregator.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeAggregator.CardsByIdsAsyncInput.Should().BeSameAs(args);
    }


    private sealed class CardAggregatorServiceFake : ICardAggregatorService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsItrEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsBySetCodeAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsBySetCodeAsyncInvokeCount { get; private set; }
        public ISetCodeItrEntity CardsBySetCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsByNameAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsByNameAsyncInvokeCount { get; private set; }
        public ICardNameItrEntity CardsByNameAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> CardNameSearchAsyncResult { get; init; } = new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardNameSearchResultCollectionItrEntityFake());
        public int CardNameSearchAsyncInvokeCount { get; private set; }
        public ICardSearchTermItrEntity CardNameSearchAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
        {
            CardsBySetCodeAsyncInvokeCount++;
            CardsBySetCodeAsyncInput = setCode;
            return Task.FromResult(CardsBySetCodeAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
        {
            CardsByNameAsyncInvokeCount++;
            CardsByNameAsyncInput = cardName;
            return Task.FromResult(CardsByNameAsyncResult);
        }

        public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
        {
            CardNameSearchAsyncInvokeCount++;
            CardNameSearchAsyncInput = searchTerm;
            return Task.FromResult(CardNameSearchAsyncResult);
        }
    }

    private sealed class CardIdsItrEntityFake : ICardIdsItrEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class CardItemCollectionItrEntityFake : ICardItemCollectionItrEntity
    {
        public ICollection<ICardItemItrEntity> Data { get; init; } = [];
    }

    private sealed class CardNameSearchResultCollectionItrEntityFake : ICardNameSearchResultCollectionItrEntity
    {
        public ICollection<ICardNameSearchResultItrEntity> Names { get; init; } = [];
    }
}