using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal sealed class RevokeAccessXfrToReadPointMapper : IRevokeAccessXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(IRevokeCollectionAccessXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.RevokerUserId)
        };

        return Task.FromResult(readPoint);
    }
}
