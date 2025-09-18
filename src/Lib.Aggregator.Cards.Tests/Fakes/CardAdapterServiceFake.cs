using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardAdapterServiceFake : ICardAdapterService
{
    public IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> GetCardsByIdsAsyncResult { get; init; } = new SuccessOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>(new List<ScryfallCardItemExtEntity>());
    public int GetCardsByIdsAsyncInvokeCount { get; private set; }
    public ICardIdsXfrEntity GetCardsByIdsAsyncArgsInput { get; private set; } = default!;

    public IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> GetCardsBySetCodeAsyncResult { get; init; } = new SuccessOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>(new List<ScryfallSetCardItemExtEntity>());
    public int GetCardsBySetCodeAsyncInvokeCount { get; private set; }
    public ISetCodeXfrEntity GetCardsBySetCodeAsyncArgsInput { get; private set; } = default!;

    public IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> GetCardsByNameAsyncResult { get; init; } = new SuccessOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>(new List<ScryfallCardByNameExtEntity>());
    public int GetCardsByNameAsyncInvokeCount { get; private set; }
    public ICardNameXfrEntity GetCardsByNameAsyncArgsInput { get; private set; } = default!;

    public IOperationResponse<IEnumerable<string>> SearchCardNamesAsyncResult { get; init; } = new SuccessOperationResponse<IEnumerable<string>>(new List<string>());
    public int SearchCardNamesAsyncInvokeCount { get; private set; }
    public ICardSearchTermXfrEntity SearchCardNamesAsyncArgsInput { get; private set; } = default!;

    public Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> GetCardsByIdsAsync(ICardIdsXfrEntity args)
    {
        GetCardsByIdsAsyncInvokeCount++;
        GetCardsByIdsAsyncArgsInput = args;
        return Task.FromResult(GetCardsByIdsAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> GetCardsBySetCodeAsync(ISetCodeXfrEntity setCode)
    {
        GetCardsBySetCodeAsyncInvokeCount++;
        GetCardsBySetCodeAsyncArgsInput = setCode;
        return Task.FromResult(GetCardsBySetCodeAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> GetCardsByNameAsync(ICardNameXfrEntity cardName)
    {
        GetCardsByNameAsyncInvokeCount++;
        GetCardsByNameAsyncArgsInput = cardName;
        return Task.FromResult(GetCardsByNameAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<string>>> SearchCardNamesAsync(ICardSearchTermXfrEntity searchTerm)
    {
        SearchCardNamesAsyncInvokeCount++;
        SearchCardNamesAsyncArgsInput = searchTerm;
        return Task.FromResult(SearchCardNamesAsyncResult);
    }
}
