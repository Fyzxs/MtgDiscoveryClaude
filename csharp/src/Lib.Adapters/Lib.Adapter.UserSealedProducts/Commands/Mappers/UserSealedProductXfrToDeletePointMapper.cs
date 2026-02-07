using System.Threading.Tasks;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserSealedProducts.Commands.Mappers;

internal sealed class UserSealedProductXfrToDeletePointMapper : IUserSealedProductXfrToDeletePointMapper
{
    public Task<DeletePointItem> Map(IUserSealedProductXfrEntity source)
    {
        DeletePointItem deletePoint = new()
        {
            Id = new ProvidedCosmosItemId(source.ProductUuid),
            Partition = new ProvidedPartitionKeyValue(source.UserId)
        };

        return Task.FromResult(deletePoint);
    }
}
