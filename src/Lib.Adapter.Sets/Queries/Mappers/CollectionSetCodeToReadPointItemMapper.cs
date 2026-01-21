using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal sealed class CollectionSetCodeToReadPointItemMapper : ICollectionSetCodeToReadPointItemMapper
{
    public Task<ICollection<ReadPointItem>> Map(IEnumerable<string> setCodes)
    {
        List<ReadPointItem> items = [];

        foreach (string setCode in setCodes)
        {
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setCode),
                Partition = new ProvidedPartitionKeyValue(setCode)
            };
            items.Add(readPoint);
        }

        return Task.FromResult<ICollection<ReadPointItem>>(items);
    }
}
