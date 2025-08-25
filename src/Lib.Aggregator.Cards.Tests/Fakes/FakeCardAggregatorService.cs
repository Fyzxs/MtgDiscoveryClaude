using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class FakeCardAggregatorService : ICardAggregatorService
{
    public IOperationResponse<ICardItemCollectionItrEntity> CardsByIdsAsyncResult { get; init; } = new FakeOperationResponse<ICardItemCollectionItrEntity>();
    public int CardsByIdsAsyncInvokeCount { get; private set; }
    public ICardIdsItrEntity CardsByIdsAsyncArgsInput { get; private set; } = default!;

    public IOperationResponse<ICardItemCollectionItrEntity> CardsBySetCodeAsyncResult { get; init; } = new FakeOperationResponse<ICardItemCollectionItrEntity>();
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
}