using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Domain.Cards.Tests.Apis;

[TestClass]
public sealed class CardDomainServiceTests
{
    private sealed class TestableCardDomainService : TypeWrapper<CardDomainService>
    {
        public TestableCardDomainService(ICardDomainService cardDomainOperations) : base(cardDomainOperations) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        CardDomainService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithValidArgs_DelegatesToOperations()
    {
        // Arrange
        CardIdsItrEntityFake args = new() { CardIds = ["id1", "id2"] };
        CardItemCollectionOufEntityFake expectedResponse = new();
        CardDomainServiceFake fakeOperations = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionOufEntity>(expectedResponse)
        };

        CardDomainService subject = new TestableCardDomainService(fakeOperations);

        // Act
        IOperationResponse<ICardItemCollectionOufEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().BeSameAs(expectedResponse);
        fakeOperations.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeOperations.CardsByIdsAsyncInput.Should().BeSameAs(args);
    }


    private sealed class CardDomainServiceFake : ICardDomainService
    {
        public IOperationResponse<ICardItemCollectionOufEntity> CardsByIdsAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionOufEntity>(new CardItemCollectionOufEntityFake());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsItrEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionOufEntity> CardsBySetCodeAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionOufEntity>(new CardItemCollectionOufEntityFake());
        public int CardsBySetCodeAsyncInvokeCount { get; private set; }
        public ISetCodeItrEntity CardsBySetCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionOufEntity> CardsByNameAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionOufEntity>(new CardItemCollectionOufEntityFake());
        public int CardsByNameAsyncInvokeCount { get; private set; }
        public ICardNameItrEntity CardsByNameAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> CardNameSearchAsyncResult { get; init; } = new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardNameSearchResultCollectionItrEntityFake());
        public int CardNameSearchAsyncInvokeCount { get; private set; }
        public ICardSearchTermItrEntity CardNameSearchAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
        {
            CardsBySetCodeAsyncInvokeCount++;
            CardsBySetCodeAsyncInput = setCode;
            return Task.FromResult(CardsBySetCodeAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
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

    private sealed class CardItemCollectionOufEntityFake : ICardItemCollectionOufEntity
    {
        public ICollection<ICardItemOufEntity> Data { get; init; } = [];
    }

    private sealed class CardNameSearchResultCollectionItrEntityFake : ICardNameSearchResultCollectionItrEntity
    {
        public ICollection<ICardNameSearchResultItrEntity> Names { get; init; } = [];
    }
}