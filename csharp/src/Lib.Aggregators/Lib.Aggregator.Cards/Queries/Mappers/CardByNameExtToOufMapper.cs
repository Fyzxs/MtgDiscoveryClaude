using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardByNameExtToOufMapper : ICardByNameExtToOufMapper
{
    private readonly IDynamicToCardItemOufEntityMapper _mapper;

    public CardByNameExtToOufMapper() : this(new DynamicToCardItemOufEntityMapper())
    { }

    private CardByNameExtToOufMapper(IDynamicToCardItemOufEntityMapper mapper) => _mapper = mapper;

    public async Task<ICardItemOufEntity> Map([NotNull] ScryfallCardByNameExtEntity source) => await _mapper.Map(source.Data).ConfigureAwait(false);
}
