using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal sealed class DeleteCollectionXfrToDeletePointMapper : IDeleteCollectionXfrToDeletePointMapper
{
    public Task<DeletePointItem> Map(IDeleteCollectionXfrEntity source)
    {
        DeletePointItem deletePoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CollectionId),
            Partition = new ProvidedPartitionKeyValue(source.OwnerId)
        };

        return Task.FromResult(deletePoint);
    }
}
