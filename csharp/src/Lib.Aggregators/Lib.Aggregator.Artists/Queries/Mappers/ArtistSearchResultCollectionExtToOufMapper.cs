using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistNameTrigrams;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Artists;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistSearchExtToOufMapper
    : ChildCollectionMapper<ArtistNameTrigramDataExtEntity, IArtistSearchResultOufEntity>,
      IArtistSearchExtToOufMapper
{
    public ArtistSearchExtToOufMapper() : this(new ArtistNameTrigramDataExtToOufEntityMapper()) { }

    internal ArtistSearchExtToOufMapper(IArtistNameTrigramDataExtToOufEntityMapper mapper) : base(mapper) { }

    public async Task<IArtistSearchResultCollectionOufEntity> Map(IEnumerable<ArtistNameTrigramDataExtEntity> source)
    {
        IArtistSearchResultOufEntity[] mappedArtists = await MapChildren(source).ConfigureAwait(false);

        IArtistSearchResultCollectionOufEntity collection = new ArtistSearchResultCollectionOufEntity
        {
            Artists = mappedArtists
        };

        return collection;
    }
}
