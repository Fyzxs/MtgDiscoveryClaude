using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal sealed class TransferOwnershipXfrToReadPointMapper : ITransferOwnershipXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(ITransferCollectionOwnershipXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.CurrentOwnerId)
        };

        return Task.FromResult(readPoint);
    }
}
