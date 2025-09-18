using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardAggregatorServiceFake : ICardAggregatorService
{
    public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } = new OperationResponseFake<ICardItemCollectionItrEntity>();
    public int CardsByIdsAsyncInvokeCount { get; private set; }
    public ICardIdsItrEntity CardsByIdsAsyncArgsInput { get; private set; } = default!;

    public IOperationResponse<ICardItemCollectionItrEntity> CardsBySetCodeAsyncResult { get; init; } = new OperationResponseFake<ICardItemCollectionItrEntity>();
    public int CardsBySetCodeAsyncInvokeCount { get; private set; }
    public ISetCodeItrEntity CardsBySetCodeAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        CardsByIdsAsyncInvokeCount++;
        CardsByIdsAsyncArgsInput = args;
        return Task.FromResult(CardsByIdsAsyncResult);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        CardsBySetCodeAsyncInvokeCount++;
        CardsBySetCodeAsyncArgsInput = setCode;
        return Task.FromResult(CardsBySetCodeAsyncResult);
    }

    public IOperationResponse<ICardItemCollectionItrEntity> CardsByNameAsyncResult { get; init; } = new OperationResponseFake<ICardItemCollectionItrEntity>();
    public int CardsByNameAsyncInvokeCount { get; private set; }
    public ICardNameItrEntity CardsByNameAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        CardsByNameAsyncInvokeCount++;
        CardsByNameAsyncArgsInput = cardName;
        return Task.FromResult(CardsByNameAsyncResult);
    }

    public IOperationResponse<ICardNameSearchResultCollectionItrEntity> CardNameSearchAsyncResult { get; init; } = new OperationResponseFake<ICardNameSearchResultCollectionItrEntity>();
    public int CardNameSearchAsyncInvokeCount { get; private set; }
    public ICardSearchTermItrEntity CardNameSearchAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
    {
        CardNameSearchAsyncInvokeCount++;
        CardNameSearchAsyncArgsInput = searchTerm;
        return Task.FromResult(CardNameSearchAsyncResult);
    }
}
