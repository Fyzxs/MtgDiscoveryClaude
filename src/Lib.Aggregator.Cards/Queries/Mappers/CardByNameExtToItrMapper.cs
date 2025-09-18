using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardByNameExtToItrMapper : ICardByNameExtToItrMapper
{
    private readonly IDynamicToCardItemItrEntityMapper _mapper;

    public CardByNameExtToItrMapper() : this(new DynamicToCardItemItrEntityMapper())
    { }

    private CardByNameExtToItrMapper(IDynamicToCardItemItrEntityMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ICardItemItrEntity> Map([NotNull] ScryfallCardByNameExtEntity source)
    {
        return await _mapper.Map(source.Data).ConfigureAwait(false);
    }
}
