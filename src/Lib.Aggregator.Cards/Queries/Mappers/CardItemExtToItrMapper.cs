using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardItemExtToItrMapper : ICardItemExtToItrMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _mapper;

    public CardItemExtToItrMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private CardItemExtToItrMapper(IDynamicToCardItemItrEntityMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ICardItemItrEntity> Map(ScryfallCardItemExtEntity source)
    {
        return await _mapper.Map(source).ConfigureAwait(false);
    }
}
