using System.Collections.Generic;
using System.Linq;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class QuerySetsCodesToReadPointItemsMapper
{
    public IEnumerable<ReadPointItem> Map(ISetCodesItrEntity args)
    {
        return args.SetCodes.Select(code => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(code),
            Partition = new ProvidedPartitionKeyValue(code)
        });
    }
}