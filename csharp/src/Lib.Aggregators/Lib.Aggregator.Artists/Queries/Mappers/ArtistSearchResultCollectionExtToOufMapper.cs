using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistNameTrigrams;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistSearchExtToOufMapper : IArtistSearchExtToOufMapper
{
    private readonly IArtistNameTrigramDataExtToOufEntityMapper _mapper;

    public ArtistSearchExtToOufMapper() : this(new ArtistNameTrigramDataExtToOufEntityMapper()) { }

    internal ArtistSearchExtToOufMapper(IArtistNameTrigramDataExtToOufEntityMapper mapper) => _mapper = mapper;

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
