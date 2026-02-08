using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardByNameExtToOufMapper : ICardByNameExtToOufMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _mapper;

    public CardByNameExtToOufMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private CardByNameExtToOufMapper(IDynamicToCardItemItrEntityMapper mapper) => _mapper = mapper;

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallCardByNameExtEntity source) => await _mapper.Map(source.Data).ConfigureAwait(false);
}
