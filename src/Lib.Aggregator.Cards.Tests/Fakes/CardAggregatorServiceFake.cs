using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardAggregatorServiceFake : ICardAggregatorService
{
    public IOperationResponse<ICardItemCollectionOufEntity> CardsByIdsAsyncResult { get; init; } = new OperationResponseFake<ICardItemCollectionOufEntity>();
    public int CardsByIdsAsyncInvokeCount { get; private set; }
    public ICardIdsItrEntity CardsByIdsAsyncArgsInput { get; private set; } = default!;

    public IOperationResponse<ICardItemCollectionOufEntity> CardsBySetCodeAsyncResult { get; init; } = new OperationResponseFake<ICardItemCollectionOufEntity>();
    public int CardsBySetCodeAsyncInvokeCount { get; private set; }
    public ISetCodeItrEntity CardsBySetCodeAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        CardsByIdsAsyncInvokeCount++;
        CardsByIdsAsyncArgsInput = args;
        return Task.FromResult(CardsByIdsAsyncResult);
    }

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        CardsBySetCodeAsyncInvokeCount++;
        CardsBySetCodeAsyncArgsInput = setCode;
        return Task.FromResult(CardsBySetCodeAsyncResult);
    }

    public IOperationResponse<ICardItemCollectionOufEntity> CardsByNameAsyncResult { get; init; } = new OperationResponseFake<ICardItemCollectionOufEntity>();
    public int CardsByNameAsyncInvokeCount { get; private set; }
    public ICardNameItrEntity CardsByNameAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        CardsByNameAsyncInvokeCount++;
        CardsByNameAsyncArgsInput = cardName;
        return Task.FromResult(CardsByNameAsyncResult);
    }

    public IOperationResponse<ICardNameSearchCollectionOufEntity> CardNameSearchAsyncResult { get; init; } = new OperationResponseFake<ICardNameSearchCollectionOufEntity>();
    public int CardNameSearchAsyncInvokeCount { get; private set; }
    public ICardSearchTermItrEntity CardNameSearchAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
    {
        CardNameSearchAsyncInvokeCount++;
        CardNameSearchAsyncArgsInput = searchTerm;
        return Task.FromResult(CardNameSearchAsyncResult);
    }
}
