using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardMetadataMapper : IUserCardMetadataMapper
{
    public UserCardExtEntity Map(UserCardExtEntity existing, IAddUserCardXfrEntity newData, IEnumerable<UserCardDetailsExtEntity> updatedCollectedList)
    {
        // Return updated item with fresh metadata from newData (including denormalized display data)
        return new UserCardExtEntity
        {
            UserId = existing.UserId,
            CardId = existing.CardId,
            SetId = existing.SetId,
            CardName = newData.CardName,
            SetName = newData.SetName,
            SetCode = newData.SetCode,
            ReleasedAt = newData.ReleasedAt,
            Artist = newData.Artist,
            ArtistIds = newData.ArtistIds,
            CardNameGuid = newData.CardNameGuid,
            CollectedList = updatedCollectedList,
            ETag = existing.ETag,  // Preserve ETag for optimistic concurrency
            CreatedDate = existing.CreatedDate  // Preserve creation date
        };
    }
}
