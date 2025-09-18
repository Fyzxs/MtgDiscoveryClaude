using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistSearchExtToItrMapper : IArtistSearchExtToItrMapper
{
    private readonly IArtistNameTrigramDataExtToItrEntityMapper _mapper;

    public ArtistSearchExtToItrMapper() : this(new ArtistNameTrigramDataExtToItrEntityMapper()) { }

    internal ArtistSearchExtToItrMapper(IArtistNameTrigramDataExtToItrEntityMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IArtistSearchResultCollectionOufEntity> Map(IEnumerable<ArtistNameTrigramDataExtEntity> source)
    {
        IArtistSearchResultItrEntity[] mappedArtists = await Task.WhenAll(
            source.Select(extEntity => _mapper.Map(extEntity))
        ).ConfigureAwait(false);

        IArtistSearchResultCollectionOufEntity collection = new ArtistSearchResultCollectionOufEntity
        {
            Artists = mappedArtists
        };

        return collection;
    }
}
