using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

/// <summary>
/// Maps existing collected items with new card details by merging counts for matching finish/special combinations.
/// </summary>
internal interface ICollectedItemsMergeMapper
{
    /// <summary>
    /// Merges the new card details into the existing collected items list, adding counts for matching finish/special combinations.
    /// </summary>
    /// <param name="existingItems">The current list of collected card details.</param>
    /// <param name="newItem">The new card details to merge in.</param>
    /// <returns>Updated collection with merged counts.</returns>
    ICollection<UserCardDetailsExtEntity> Map(ICollection<UserCardDetailsExtEntity> existingItems, IUserCardDetailsXfrEntity newItem);
}
