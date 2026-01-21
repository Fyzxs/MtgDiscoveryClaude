using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal sealed class CollectionSetIdToReadPointItemMapper : ICollectionSetIdToReadPointItemMapper
{
    public Task<ICollection<ReadPointItem>> Map(IEnumerable<string> setIds)
    {
        List<ReadPointItem> items = [];

        foreach (string setId in setIds)
        {
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setId),
                Partition = new ProvidedPartitionKeyValue(setId)
            };
            items.Add(readPoint);
        }

        return Task.FromResult<ICollection<ReadPointItem>>(items);
    }
}
