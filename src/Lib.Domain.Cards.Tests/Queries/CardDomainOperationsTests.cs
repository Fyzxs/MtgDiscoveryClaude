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
using TestConvenience.Core.Reflection;

namespace Lib.Domain.Cards.Tests.Queries;

[TestClass]
public sealed class CardDomainOperationsTests
{
    private sealed class TestableCardDomainOperations : TypeWrapper<CardDomainOperations>
    {
        public TestableCardDomainOperations(ICardAggregatorService cardAggregatorService) : base(cardAggregatorService) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        CardDomainOperations _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithValidArgs_DelegatesToAggregatorService()
    {
        // Arrange
        FakeCardIdsItrEntity args = new() { CardIds = ["id1", "id2"] };
        FakeCardItemCollectionItrEntity expectedResponse = new();
        FakeCardAggregatorService fakeAggregator = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };

        CardDomainOperations subject = new TestableCardDomainOperations(fakeAggregator);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().BeSameAs(expectedResponse);
        fakeAggregator.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeAggregator.CardsByIdsAsyncInput.Should().BeSameAs(args);
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

    private sealed class FakeCardAggregatorService : ICardAggregatorService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new FakeCardItemCollectionItrEntity());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsItrEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }
    }

    private sealed class FakeCardIdsItrEntity : ICardIdsItrEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class FakeCardItemCollectionItrEntity : ICardItemCollectionItrEntity
    {
        public ICollection<ICardItemItrEntity> Data { get; init; } = [];
    }
}