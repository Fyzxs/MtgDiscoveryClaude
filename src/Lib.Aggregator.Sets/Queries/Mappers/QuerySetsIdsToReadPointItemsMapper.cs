using System.Collections.Generic;
using System.Linq;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class QuerySetsIdsToReadPointItemsMapper
{
    public IEnumerable<ReadPointItem> Map(ISetIdsItrEntity args)
    {
        return args.SetIds.Select(id => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(id),
            Partition = new ProvidedPartitionKeyValue(id)
        });
    }
}