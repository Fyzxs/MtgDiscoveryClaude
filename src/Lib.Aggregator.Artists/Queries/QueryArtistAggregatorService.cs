using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Aggregator.Artists.Apis;
using Lib.Aggregator.Artists.Operations;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class QueryArtistAggregatorService : IArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;

    public QueryArtistAggregatorService(ILogger logger) : this(new ArtistAdapterService(logger))
    { }

    private QueryArtistAggregatorService(
        IArtistAdapterService artistAdapterService)
    {
        _artistAdapterService = artistAdapterService;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        return await _artistAdapterService.SearchArtistsAsync(searchTerm).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        return await _artistAdapterService.GetCardsByArtistIdAsync(artistId).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        return await _artistAdapterService.GetCardsByArtistNameAsync(artistName).ConfigureAwait(false);
    }
}
