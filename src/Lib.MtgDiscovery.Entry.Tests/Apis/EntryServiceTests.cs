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

        public IOperationResponse<ICardItemCollectionItrEntity> CardsBySetCodeAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new FakeCardItemCollectionItrEntity());
        public int CardsBySetCodeAsyncInvokeCount { get; private set; }
        public ISetCodeArgEntity CardsBySetCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsByNameAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new FakeCardItemCollectionItrEntity());
        public int CardsByNameAsyncInvokeCount { get; private set; }
        public ICardNameArgEntity CardsByNameAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> CardNameSearchAsyncResult { get; init; } = new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new FakeCardNameSearchResultCollectionItrEntity());
        public int CardNameSearchAsyncInvokeCount { get; private set; }
        public ICardSearchTermArgEntity CardNameSearchAsyncInput { get; private set; } = default!;

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
        {
            CardsByIdsAsyncInvokeCount++;
            CardsByIdsAsyncInput = args;
            return Task.FromResult(CardsByIdsAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode)
        {
            CardsBySetCodeAsyncInvokeCount++;
            CardsBySetCodeAsyncInput = setCode;
            return Task.FromResult(CardsBySetCodeAsyncResult);
        }

        public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameArgEntity cardName)
        {
            CardsByNameAsyncInvokeCount++;
            CardsByNameAsyncInput = cardName;
            return Task.FromResult(CardsByNameAsyncResult);
        }

        public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm)
        {
            CardNameSearchAsyncInvokeCount++;
            CardNameSearchAsyncInput = searchTerm;
            return Task.FromResult(CardNameSearchAsyncResult);
        }
    }

    private sealed class FakeSetEntryService : ISetEntryService
    {
        public IOperationResponse<ISetItemCollectionItrEntity> SetsByIdsAsyncResult { get; init; } = new SuccessOperationResponse<ISetItemCollectionItrEntity>(new FakeSetItemCollectionItrEntity());
        public int SetsByIdsAsyncInvokeCount { get; private set; }
        public ISetIdsArgEntity SetsByIdsAsyncInput { get; private set; } = default!;

        public IOperationResponse<ISetItemCollectionItrEntity> SetsByCodeAsyncResult { get; init; } = new SuccessOperationResponse<ISetItemCollectionItrEntity>(new FakeSetItemCollectionItrEntity());
        public int SetsByCodeAsyncInvokeCount { get; private set; }
        public ISetCodesArgEntity SetsByCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ISetItemCollectionItrEntity> AllSetsAsyncResult { get; init; } = new SuccessOperationResponse<ISetItemCollectionItrEntity>(new FakeSetItemCollectionItrEntity());
        public int AllSetsAsyncInvokeCount { get; private set; }

        public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByIdsAsync(ISetIdsArgEntity setIds)
        {
            SetsByIdsAsyncInvokeCount++;
            SetsByIdsAsyncInput = setIds;
            return Task.FromResult(SetsByIdsAsyncResult);
        }

        public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesArgEntity setCodes)
        {
            SetsByCodeAsyncInvokeCount++;
            SetsByCodeAsyncInput = setCodes;
            return Task.FromResult(SetsByCodeAsyncResult);
        }

        public Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
        {
            AllSetsAsyncInvokeCount++;
            return Task.FromResult(AllSetsAsyncResult);
        }
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }

    private sealed class FakeCardItemCollectionItrEntity : ICardItemCollectionItrEntity
    {
        public ICollection<ICardItemItrEntity> Data { get; init; } = [];
    }

    private sealed class FakeCardNameSearchResultCollectionItrEntity : ICardNameSearchResultCollectionItrEntity
    {
        public ICollection<ICardNameSearchResultItrEntity> Names { get; init; } = [];
    }

    private sealed class FakeSetItemCollectionItrEntity : ISetItemCollectionItrEntity
    {
        public ICollection<ISetItemItrEntity> Data { get; init; } = [];
    }
}
