using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Mappers;

internal sealed class ArtistCardExtToItrEntityMapper : IArtistCardExtToItrEntityMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _dynamicMapper;

    public ArtistCardExtToItrEntityMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private ArtistCardExtToItrEntityMapper(IDynamicToCardItemItrEntityMapper dynamicMapper)
    {
        _dynamicMapper = dynamicMapper;
    }

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallArtistCardExtEntity source)
    {
        return await _dynamicMapper.Map(source.Data).ConfigureAwait(false);
    }
}
