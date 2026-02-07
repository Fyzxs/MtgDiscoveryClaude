using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal sealed class GrantAccessXfrToReadPointMapper : IGrantAccessXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(IGrantCollectionAccessXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.GrantorUserId)
        };

        return Task.FromResult(readPoint);
    }
}
