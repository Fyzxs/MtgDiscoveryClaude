using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistSearchExtToItrMapper : IArtistSearchExtToItrMapper
{
    private readonly IArtistNameTrigramDataExtToItrEntityMapper _artistSearchToItrMapper;

    public ArtistSearchExtToItrMapper() : this(new ArtistNameTrigramDataExtToItrEntityMapper()) { }

    internal ArtistSearchExtToItrMapper(IArtistNameTrigramDataExtToItrEntityMapper artistSearchToItrMapper)
    {
        _artistSearchToItrMapper = artistSearchToItrMapper;
    }

    public async Task<IArtistSearchResultCollectionItrEntity> Map(IEnumerable<ArtistNameTrigramDataExtEntity> source)
    {
        List<IArtistSearchResultItrEntity> mappedArtists = [];
        foreach (ArtistNameTrigramDataExtEntity extEntity in source)
        {
            IArtistSearchResultItrEntity itrEntity = await _artistSearchToItrMapper.Map(extEntity).ConfigureAwait(false);
            mappedArtists.Add(itrEntity);
        }

        IArtistSearchResultCollectionItrEntity collection = new ArtistSearchResultCollectionItrEntity
        {
            Artists = mappedArtists
        };

        return collection;
    }
}
