using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Internals;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Apis;

/// <summary>
/// Main card adapter service implementation following the passthrough pattern.
/// 
/// This service coordinates all card-related adapter operations by delegating
/// to specialized adapters. Currently delegates to ICardQueryAdapter for all
/// operations, but provides the architectural structure for future expansion.
/// 
/// Future Expansion Examples:
///   - ICardCacheAdapter for caching layer
///   - ICardFallbackAdapter for redundancy
///   - ICardMetricsAdapter for telemetry
/// 
/// Pattern Consistency:
/// Matches EntryService, DomainService, and AggregatorService patterns
/// to maintain predictable architecture across all layers.
/// </summary>
public sealed class CardAdapterService : ICardAdapterService
{
    private readonly ICardQueryAdapter _cardQueryAdapter;

    public CardAdapterService(ILogger logger) : this(new CardCosmosQueryAdapter(logger))
    { }

    private CardAdapterService(ICardQueryAdapter cardQueryAdapter)
    {
        _cardQueryAdapter = cardQueryAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsByIdsAsync(ICardIdsItrEntity cardIds)
    {
        return await _cardQueryAdapter.GetCardsByIdsAsync(cardIds).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        return await _cardQueryAdapter.GetCardsBySetCodeAsync(setCode).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsByNameAsync(ICardNameItrEntity cardName)
    {
        return await _cardQueryAdapter.GetCardsByNameAsync(cardName).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ICardNameSearchResultItrEntity>>> SearchCardNamesAsync(ICardSearchTermItrEntity searchTerm)
    {
        return await _cardQueryAdapter.SearchCardNamesAsync(searchTerm).ConfigureAwait(false);
    }
}