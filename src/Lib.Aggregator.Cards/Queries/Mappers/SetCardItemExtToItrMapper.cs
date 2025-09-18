using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class SetCardItemExtToItrMapper : ISetCardItemExtToItrMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _dynamicMapper;

    public SetCardItemExtToItrMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private SetCardItemExtToItrMapper(IDynamicToCardItemItrEntityMapper dynamicMapper)
    {
        _dynamicMapper = dynamicMapper;
    }

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallSetCardItemExtEntity source)
    {
        return await _dynamicMapper.Map(source.Data).ConfigureAwait(false);
    }
}
