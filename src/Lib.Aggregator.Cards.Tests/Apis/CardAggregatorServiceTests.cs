using System;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Cards.Tests.Apis;

[TestClass]
public sealed class CardAggregatorServiceTests
{
    private sealed class TestableCardAggregatorService : TypeWrapper<CardAggregatorService>
    {
        public TestableCardAggregatorService(ICardAggregatorService cardAggregatorOperations) : base(cardAggregatorOperations) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        CardAggregatorService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_CallsDependency()
    {
        // Arrange
        FakeCardIdsItrEntity args = new()
        {
            CardIds = ["id1", "id2"]
        };

        FakeCardItemCollectionItrEntity expectedCollection = new();
        FakeOperationResponse<ICardItemCollectionItrEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedCollection
        };

        FakeCardAggregatorService fakeOperations = new()
        {
            CardsByIdsAsyncResult = expectedResponse
        };

        CardAggregatorService subject = new TestableCardAggregatorService(fakeOperations);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeSameAs(expectedResponse);
        fakeOperations.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeOperations.CardsByIdsAsyncArgsInput.Should().BeSameAs(args);
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
}
