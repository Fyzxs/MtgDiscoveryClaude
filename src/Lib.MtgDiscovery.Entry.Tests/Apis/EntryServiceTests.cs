using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;
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
        CardIdsArgEntityFake args = new() { CardIds = ["id1", "id2"] };
        CardItemCollectionItrEntityFake expectedResponse = new();
        CardEntryServiceFake fakeCardEntryService = new()
        {
            CardsByIdsAsyncResult = new SuccessOperationResponse<ICardItemCollectionItrEntity>(expectedResponse)
        };
        SetEntryServiceFake fakeSetEntryService = new();

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


    private sealed class CardEntryServiceFake : ICardEntryService
    {
        public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } =
            new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsByIdsAsyncInvokeCount { get; private set; }
        public ICardIdsArgEntity CardsByIdsAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsBySetCodeAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsBySetCodeAsyncInvokeCount { get; private set; }
        public ISetCodeArgEntity CardsBySetCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardItemCollectionItrEntity> CardsByNameAsyncResult { get; init; } = new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntityFake());
        public int CardsByNameAsyncInvokeCount { get; private set; }
        public ICardNameArgEntity CardsByNameAsyncInput { get; private set; } = default!;

        public IOperationResponse<ICardNameSearchResultCollectionItrEntity> CardNameSearchAsyncResult { get; init; } = new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardNameSearchResultCollectionItrEntityFake());
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

    private sealed class SetEntryServiceFake : ISetEntryService
    {
        public IOperationResponse<ISetItemCollectionItrEntity> SetsByIdsAsyncResult { get; init; } = new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntityFake());
        public int SetsByIdsAsyncInvokeCount { get; private set; }
        public ISetIdsArgEntity SetsByIdsAsyncInput { get; private set; } = default!;

        public IOperationResponse<ISetItemCollectionItrEntity> SetsByCodeAsyncResult { get; init; } = new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntityFake());
        public int SetsByCodeAsyncInvokeCount { get; private set; }
        public ISetCodesArgEntity SetsByCodeAsyncInput { get; private set; } = default!;

        public IOperationResponse<ISetItemCollectionItrEntity> AllSetsAsyncResult { get; init; } = new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntityFake());
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

    private sealed class CardIdsArgEntityFake : ICardIdsArgEntity
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

    private sealed class SetItemCollectionItrEntityFake : ISetItemCollectionItrEntity
    {
        public ICollection<ISetItemItrEntity> Data { get; init; } = [];
    }
}
