using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class SetCardItemExtToOufMapper : ISetCardItemExtToOufMapper
{
    private readonly IDynamicToCardItemOufEntityMapper _mapper;

    public SetCardItemExtToOufMapper() : this(new DynamicToCardItemOufEntityMapper())
    { }

    private SetCardItemExtToOufMapper(IDynamicToCardItemOufEntityMapper mapper) => _mapper = mapper;

    public async Task<ICardItemOufEntity> Map([NotNull] ScryfallSetCardItemExtEntity source) => await _mapper.Map(source.Data).ConfigureAwait(false);
}
