using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class AddUserCardXfrToReadPointMapper : IAddUserCardXfrToReadPointMapper
{
    public Task<ReadPointItem> Map(IAddUserCardXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.CardId),
            Partition = new ProvidedPartitionKeyValue(source.UserId)
        };

        return Task.FromResult(readPoint);
    }
}
