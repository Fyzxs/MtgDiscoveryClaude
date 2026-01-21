using System.Threading.Tasks;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Mappers.UserSealedProducts;

/// <summary>
/// Maps sealed product UUID and user ID to a ReadPointItem for Cosmos DB operations.
/// </summary>
internal sealed class UserSealedProductReadPointMapper : IUserSealedProductReadPointMapper
{
    public UserSealedProductReadPointMapper()
    {
    }

    public Task<ReadPointItem> Map(string productUuid, string userId)
    {
        ReadPointItem readPoint = new ReadPointItem
        {
            Id = new ProvidedCosmosItemId(productUuid),
            Partition = new ProvidedPartitionKeyValue(userId)
        };

        return Task.FromResult(readPoint);
    }
}
