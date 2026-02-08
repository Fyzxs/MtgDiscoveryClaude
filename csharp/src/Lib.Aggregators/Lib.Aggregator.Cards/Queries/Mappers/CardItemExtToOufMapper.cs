using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardItemExtToOufMapper : ICardItemExtToOufMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _mapper;

    public CardItemExtToOufMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private CardItemExtToOufMapper(IDynamicToCardItemItrEntityMapper mapper) => _mapper = mapper;

    public async Task<ICardItemItrEntity> Map(ScryfallCardItemExtEntity source) => await _mapper.Map(source.Data).ConfigureAwait(false);
}
