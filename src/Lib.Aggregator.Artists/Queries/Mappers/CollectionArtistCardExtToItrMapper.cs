using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class CollectionArtistCardExtToItrMapper : ICollectionArtistCardExtToItrMapper
{
    private readonly IArtistCardExtToItrEntityMapper _mapper;

    public CollectionArtistCardExtToItrMapper() : this(new ArtistCardExtToItrEntityMapper())
    { }

    private CollectionArtistCardExtToItrMapper(IArtistCardExtToItrEntityMapper mapper) => _mapper = mapper;

    public async Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallArtistCardExtEntity> source)
    {
        ICollection<Task<ICardItemItrEntity>> tasks = [.. source.Select(item => _mapper.Map(item))];
        ICardItemItrEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
