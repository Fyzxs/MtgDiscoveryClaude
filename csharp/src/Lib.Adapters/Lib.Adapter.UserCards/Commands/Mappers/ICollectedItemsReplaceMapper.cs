using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

/// <summary>
/// Maps existing collected items with new card details by replacing counts for matching finish/special combinations.
/// Used by migration tools to overwrite existing collection data.
/// </summary>
internal interface ICollectedItemsReplaceMapper
{
    /// <summary>
    /// Replaces the count for the matching finish/special combination in the existing collected items list.
    /// </summary>
    /// <param name="existingItems">The current list of collected card details.</param>
    /// <param name="newItem">The new card details to replace with.</param>
    /// <returns>Updated collection with replaced counts.</returns>
    ICollection<UserCardDetailsExtEntity> Map(ICollection<UserCardDetailsExtEntity> existingItems, IUserCardDetailsXfrEntity newItem);
}
