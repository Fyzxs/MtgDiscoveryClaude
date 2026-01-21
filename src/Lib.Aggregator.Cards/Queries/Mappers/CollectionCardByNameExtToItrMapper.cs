using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CollectionCardByNameExtToItrMapper : ICollectionCardByNameExtToItrMapper
{
    private readonly ICardByNameExtToItrMapper _mapper;

    public CollectionCardByNameExtToItrMapper() : this(new CardByNameExtToItrMapper())
    { }

    private CollectionCardByNameExtToItrMapper(ICardByNameExtToItrMapper mapper) => _mapper = mapper;

    public async Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallCardByNameExtEntity> source)
    {
        ICollection<Task<ICardItemItrEntity>> tasks = [.. source.Select(item => _mapper.Map(item))];
        ICardItemItrEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
