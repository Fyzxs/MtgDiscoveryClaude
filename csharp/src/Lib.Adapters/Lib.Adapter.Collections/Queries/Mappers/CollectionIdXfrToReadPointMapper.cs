using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal sealed class CollectionIdXfrToReadPointMapper : ICollectionIdXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(ICollectionIdXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.OwnerId)
        };

        return Task.FromResult(readPoint);
    }
}
