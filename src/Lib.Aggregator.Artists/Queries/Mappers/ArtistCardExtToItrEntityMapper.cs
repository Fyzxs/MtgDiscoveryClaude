using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistCardExtToItrEntityMapper : IArtistCardExtToItrEntityMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _mapper;

    public ArtistCardExtToItrEntityMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private ArtistCardExtToItrEntityMapper(IDynamicToCardItemItrEntityMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallArtistCardExtEntity source)
    {
        return await _mapper.Map(source.Data).ConfigureAwait(false);
    }
}
