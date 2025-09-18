using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class CollectionSetCodeItrToReadPointItemMapper : ICollectionSetCodeItrToReadPointItemMapper
{
    public Task<IEnumerable<ReadPointItem>> Map(ISetCodesItrEntity args)
    {
        return Task.FromResult(args.SetCodes.Select(code => new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(code),
            Partition = new ProvidedPartitionKeyValue(code)
        }));
    }
}
