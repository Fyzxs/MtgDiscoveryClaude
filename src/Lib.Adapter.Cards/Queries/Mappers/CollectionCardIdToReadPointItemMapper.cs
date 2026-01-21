using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Cards.Queries.Mappers;

internal sealed class CollectionCardIdToReadPointItemMapper : ICollectionCardIdToReadPointItemMapper
{
    public Task<ICollection<ReadPointItem>> Map(IEnumerable<string> cardIds)
    {
        IEnumerable<ReadPointItem> readPointItems = cardIds.Select(cardId => new ReadPointItem() { Id = new ProvidedCosmosItemId(cardId), Partition = new ProvidedPartitionKeyValue(cardId) });

        return Task.FromResult<ICollection<ReadPointItem>>([.. readPointItems]);
    }
}
