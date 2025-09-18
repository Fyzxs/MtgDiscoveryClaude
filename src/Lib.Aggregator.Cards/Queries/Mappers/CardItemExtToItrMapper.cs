using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardItemExtToItrMapper : ICardItemExtToItrMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _dynamicMapper;

    public CardItemExtToItrMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private CardItemExtToItrMapper(IDynamicToCardItemItrEntityMapper dynamicMapper)
    {
        _dynamicMapper = dynamicMapper;
    }

    public async Task<ICardItemItrEntity> Map(ScryfallCardItemExtEntity source)
    {
        return await _dynamicMapper.Map(source).ConfigureAwait(false);
    }
}
