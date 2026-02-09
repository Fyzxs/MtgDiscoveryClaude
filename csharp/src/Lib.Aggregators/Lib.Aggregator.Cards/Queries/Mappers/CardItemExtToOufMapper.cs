using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardItemExtToOufMapper : ICardItemExtToOufMapper
{
    private readonly IDynamicToCardItemOufEntityMapper _mapper;

    public CardItemExtToOufMapper() : this(new DynamicToCardItemOufEntityMapper())
    { }

    private CardItemExtToOufMapper(IDynamicToCardItemOufEntityMapper mapper) => _mapper = mapper;

    public async Task<ICardItemOufEntity> Map(ScryfallCardItemExtEntity source) => await _mapper.Map(source.Data).ConfigureAwait(false);
}
