using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class AddUserCardXfrToExtMapper : IAddUserCardXfrToExtMapper
{
    public Task<UserCardExtEntity> Map(IAddUserCardXfrEntity source)
    {
        IUserCardDetailsXfrEntity item = source.Details;

        // Note: setGroupId from item is used to update UserSetCards aggregation
        // but is NOT persisted in the UserCard record itself
        UserCardDetailsExtEntity collectedItem = new()
        {
            Finish = item.Finish,
            Special = item.Special,
            Count = item.Count
            // setGroupId intentionally omitted - used for aggregation only
        };

        UserCardExtEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            ArtistIds = source.ArtistIds,
            CardNameGuid = source.CardNameGuid,
            CollectedList = [collectedItem]
        };

        return Task.FromResult(result);
    }
}
