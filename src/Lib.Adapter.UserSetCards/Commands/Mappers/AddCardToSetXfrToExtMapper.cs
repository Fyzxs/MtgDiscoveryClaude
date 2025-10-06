using System.Threading.Tasks;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal sealed class AddCardToSetXfrToExtMapper : IAddCardToSetXfrToExtMapper
{
    public Task<ReadPointItem> Map(IAddCardToSetXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.SetId),
            Partition = new ProvidedPartitionKeyValue(source.UserId)
        };

        return Task.FromResult(readPoint);
    }
}
