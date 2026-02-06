using System.Threading.Tasks;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserSealedProducts.Commands.Mappers;

internal sealed class UserSealedProductXfrToReadPointMapper : IUserSealedProductXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(IUserSealedProductXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.ProductUuid),
            Partition = new ProvidedPartitionKeyValue(source.UserId)
        };

        return Task.FromResult(readPoint);
    }
}
