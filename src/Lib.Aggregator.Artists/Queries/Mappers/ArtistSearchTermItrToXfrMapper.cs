using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Scryfall.Shared.Entities;
using Lib.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistSearchTermItrToXfrMapper : IArtistSearchTermItrToXfrMapper
{
    private readonly ISearchTermToTrigramsMapper _trigramMapper;

    public ArtistSearchTermItrToXfrMapper() : this(new SearchTermToTrigramsMapper())
    {
    }

    private ArtistSearchTermItrToXfrMapper(ISearchTermToTrigramsMapper trigramMapper) => _trigramMapper = trigramMapper;

    public async Task<IArtistSearchTermXfrEntity> Map(IArtistSearchTermItrEntity source)
    {
        ITrigramCollectionEntity trigramCollection = await _trigramMapper.Map(source.SearchTerm).ConfigureAwait(false);

        ArtistSearchTermXfrEntity mapped = new()
        {
            Normalized = trigramCollection.Normalized,
            SearchTerms = [.. trigramCollection.Trigrams]
        };

        return mapped;
    }
}
