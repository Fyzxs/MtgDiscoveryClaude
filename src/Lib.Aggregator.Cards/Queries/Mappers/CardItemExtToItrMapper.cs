using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities;

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

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallCardItemExtEntity source)
    {
        return await _dynamicMapper.Map(source.Data).ConfigureAwait(false);
    }
}
