using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CollectionCardItemExtToItrMapper : ICollectionCardItemExtToItrMapper
{
    private readonly ICardItemExtToItrMapper _mapper;

    public CollectionCardItemExtToItrMapper() : this(new CardItemExtToItrMapper())
    { }

    private CollectionCardItemExtToItrMapper(ICardItemExtToItrMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallCardItemExtEntity> source)
    {
        ICollection<Task<ICardItemItrEntity>> tasks = source.Select(item => _mapper.Map(item)).ToList();
        ICardItemItrEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
