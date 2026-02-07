using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal sealed class TransferOwnershipXfrToDeletePointMapper : ITransferOwnershipXfrToDeletePointMapper
{
    public Task<DeletePointItem> Map(ITransferCollectionOwnershipXfrEntity source)
    {
        DeletePointItem deletePoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.CurrentOwnerId)
        };

        return Task.FromResult(deletePoint);
    }
}
