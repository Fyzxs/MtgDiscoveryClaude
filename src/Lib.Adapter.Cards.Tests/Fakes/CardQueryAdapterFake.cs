using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Cards.Tests.Fakes;

internal sealed class CardQueryAdapterFake : ICardQueryAdapter
{
    public IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> GetCardsByIdsAsyncResult { get; init; }
    public int GetCardsByIdsAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> GetCardsBySetCodeAsyncResult { get; init; }
    public int GetCardsBySetCodeAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> GetCardsByNameAsyncResult { get; init; }
    public int GetCardsByNameAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<string>> SearchCardNamesAsyncResult { get; init; }
    public int SearchCardNamesAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> GetCardsByIdsAsync(ICardIdsItrEntity cardIds)
    {
        GetCardsByIdsAsyncInvokeCount++;
        return await Task.FromResult(GetCardsByIdsAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> GetCardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        GetCardsBySetCodeAsyncInvokeCount++;
        return await Task.FromResult(GetCardsBySetCodeAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> GetCardsByNameAsync(ICardNameItrEntity cardName)
    {
        GetCardsByNameAsyncInvokeCount++;
        return await Task.FromResult(GetCardsByNameAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<string>>> SearchCardNamesAsync(ICardSearchTermItrEntity searchTerm)
    {
        SearchCardNamesAsyncInvokeCount++;
        return await Task.FromResult(SearchCardNamesAsyncResult).ConfigureAwait(false);
    }
}
