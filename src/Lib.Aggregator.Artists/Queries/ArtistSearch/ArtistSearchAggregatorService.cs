using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries.ArtistSearch;

internal sealed class ArtistSearchAggregatorService : IArtistSearchAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistSearchTermItrToXfrMapper _artistSearchItrToXfrMapper;
    private readonly IArtistSearchExtToItrMapper _artistSearchResultCollectionMapper;

    public ArtistSearchAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistSearchTermItrToXfrMapper(),
        new ArtistSearchExtToItrMapper())
    { }

    private ArtistSearchAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistSearchTermItrToXfrMapper artistSearchItrToXfrMapper,
        IArtistSearchExtToItrMapper artistSearchResultCollectionMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistSearchItrToXfrMapper = artistSearchItrToXfrMapper;
        _artistSearchResultCollectionMapper = artistSearchResultCollectionMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        IArtistSearchTermXfrEntity mappedEntity = await _artistSearchItrToXfrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>> adapterResponse = await _artistAdapterService.SearchArtistsAsync(mappedEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<IArtistSearchResultCollectionOufEntity>(adapterResponse.OuterException);
        }

        IArtistSearchResultCollectionOufEntity collection = await _artistSearchResultCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IArtistSearchResultCollectionOufEntity>(collection);
    }
}
