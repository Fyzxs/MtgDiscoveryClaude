using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

/// <summary>
/// Maps existing UserCardExtEntity with new metadata and collected items list.
/// </summary>
internal interface IUserCardMetadataMapper
{
    /// <summary>
    /// Updates the user card entity with fresh metadata from the new data while preserving core identity and concurrency fields.
    /// </summary>
    /// <param name="existing">The existing user card entity.</param>
    /// <param name="newData">The new card data with updated metadata.</param>
    /// <param name="updatedCollectedList">The updated collected items list.</param>
    /// <returns>Updated user card entity with fresh metadata.</returns>
    UserCardExtEntity Map(UserCardExtEntity existing, IAddUserCardXfrEntity newData, IEnumerable<UserCardDetailsExtEntity> updatedCollectedList);
}
