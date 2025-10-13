using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Scryfall.Shared.Entities;
using Lib.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistNameItrToXfrMapper : IArtistNameItrToXfrMapper
{
    private readonly ISearchTermToTrigramsMapper _trigramMapper;

    public ArtistNameItrToXfrMapper() : this(new SearchTermToTrigramsMapper())
    {
    }

    private ArtistNameItrToXfrMapper(ISearchTermToTrigramsMapper trigramMapper) => _trigramMapper = trigramMapper;

    public async Task<IArtistNameXfrEntity> Map(IArtistNameItrEntity source)
    {
        ITrigramCollectionEntity trigramCollection = await _trigramMapper.Map(source.ArtistName).ConfigureAwait(false);

        return new ArtistNameXfrEntity
        {
            ArtistName = source.ArtistName,
            Normalized = trigramCollection.Normalized,
            Trigrams = [.. trigramCollection.Trigrams]
        };
    }
}
