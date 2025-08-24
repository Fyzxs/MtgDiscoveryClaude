using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.MtgDiscovery.Entry.Tests.Apis;

[TestClass]
public sealed class EntryServiceTests
{
    private sealed class TestableEntryService : TypeWrapper<EntryService>
    {
        public TestableEntryService(ICardEntryService cardEntryService, ISetEntryService setEntryService) 
            : base(cardEntryService, setEntryService) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        ILogger logger = new LoggerFake();

        // Act
        EntryService _ = new(logger);

        // Assert
        // Constructor should create instance without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task CardsByIdsAsync_WithValidArgs_DelegatesToCardEntryService()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "id2"] };
        FakeCardItemCollectionItrEntity expectedResponse = new();
        FakeCardEntryService fakeCardEntryService = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };
        FakeSetEntryService fakeSetEntryService = new();

        EntryService subject = new TestableEntryService(fakeCardEntryService, fakeSetEntryService);

        // Act
        IOperationResponse<ICardItemCollectionItrEntity> actual = await subject.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.Should().BeSameAs(expectedResponse);
        fakeCardEntryService.CardsByIdsAsyncInvokeCount.Should().Be(1);
        fakeCardEntryService.CardsByIdsAsyncInput.Should().BeSameAs(args);
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

    private sealed class FakeCardEntryService : ICardEntryService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } = 
            new SuccessOperationResponse<ICardItemCollectionItrEntity>(new FakeCardItemCollectionItrEntity());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsArgEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }
    }

    private sealed class FakeSetEntryService : ISetEntryService
    {
        // Empty fake implementation as SetEntryService is not used in current EntryService
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class FakeCardItemCollectionItrEntity : ICardItemCollectionItrEntity
    {
        public ICollection<ICardItemItrEntity> Data { get; init; } = [];
    }
}