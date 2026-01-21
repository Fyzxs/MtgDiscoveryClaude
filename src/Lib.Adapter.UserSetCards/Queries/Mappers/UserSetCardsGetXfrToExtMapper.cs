using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardsGetXfrToExtMapper : IUserSetCardsGetXfrToExtMapper
{
    public Task<ReadPointItem> Map(IUserSetCardGetXfrEntity source)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(source.SetId),
            Partition = new ProvidedPartitionKeyValue(source.UserId)
        };

        return Task.FromResult(readPoint);
    }
}
