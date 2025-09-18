using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class CollectionSetsIdToReadPointItemMapper : ICollectionSetsIdToReadPointItemMapper
{
    public Task<IEnumerable<ReadPointItem>> Map(ISetIdsItrEntity args)
    {
        return Task.FromResult(args.SetIds.Select(id => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(id),
            Partition = new ProvidedPartitionKeyValue(id)
        }));
    }
}
