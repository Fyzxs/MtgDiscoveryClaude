using System.Threading.Tasks;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserWishlistCards.Commands.Mappers;

internal sealed class RemoveUserWishlistCardXfrToReadPointMapper : IRemoveUserWishlistCardXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(IRemoveUserWishlistCardXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CardId),
            Partition = new ProvidedPartitionKeyValue(source.UserId)
        };

        return Task.FromResult(readPoint);
    }
}
