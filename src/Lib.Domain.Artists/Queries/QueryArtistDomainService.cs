using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Domain.Artists.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Queries;

internal sealed class QueryArtistDomainService : IArtistDomainService
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public QueryArtistDomainService(ILogger logger) : this(new ArtistAggregatorService(logger))
    {

    }

    private QueryArtistDomainService(IArtistAggregatorService artistAggregatorService)
    {
        _artistAggregatorService = artistAggregatorService;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        return await _artistAggregatorService.ArtistSearchAsync(searchTerm).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        return await _artistAggregatorService.CardsByArtistAsync(artistId).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        return await _artistAggregatorService.CardsByArtistNameAsync(artistName).ConfigureAwait(false);
    }
}