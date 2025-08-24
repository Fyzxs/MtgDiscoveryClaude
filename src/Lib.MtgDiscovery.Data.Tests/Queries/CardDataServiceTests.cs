using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Data.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.MtgDiscovery.Data.Tests.Queries;

[TestClass]
public sealed class CardDataServiceTests
{

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        CardDataService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithCardDomainService_CreatesInstance()
    {
        // Arrange
        FakeCardDomainService fakeCardDomainService = new();

        // Act
        CardDataService _ = new(fakeCardDomainService);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithValidArgs_DelegatesToCardDomainService()
    {
        // Arrange
        FakeCardIdsItrEntity args = new() { CardIds = ["id1", "id2"] };
        FakeCardItemCollectionItrEntity expectedResponse = new();
        FakeCardDomainService fakeCardDomainService = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };

        CardDataService subject = new(fakeCardDomainService);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().BeSameAs(expectedResponse);
        fakeCardDomainService.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardDomainService.CardsByIdsAsyncInput.Should().BeSameAs(args);
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

    private sealed class FakeCardDomainService : ICardDomainService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } =
            new SuccessOperationResponse<ICardItemCollectionItrEntity>(new FakeCardItemCollectionItrEntity());
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