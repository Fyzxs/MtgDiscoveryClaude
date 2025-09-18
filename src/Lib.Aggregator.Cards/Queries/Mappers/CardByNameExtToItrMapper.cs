using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardByNameExtToItrMapper : ICardByNameExtToItrMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _dynamicMapper;

    public CardByNameExtToItrMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private CardByNameExtToItrMapper(IDynamicToCardItemItrEntityMapper dynamicMapper)
    {
        _dynamicMapper = dynamicMapper;
    }

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallCardByNameExtEntity source)
    {
        return await _dynamicMapper.Map(source.Data).ConfigureAwait(false);
    }
}
