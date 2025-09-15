using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

/// <summary>
/// Represents a user's card collection entry with card details and collection information.
/// </summary>
public interface IUserCardCollectionItrEntity
{
    /// <summary>
    /// The unique identifier of the user who owns this card collection entry.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier of the card in the collection.
    /// </summary>
    string CardId { get; }

    /// <summary>
    /// The identifier of the set this card belongs to.
    /// </summary>
    string SetId { get; }

    /// <summary>
    /// The list of collected versions of this card with quantities and finishes.
    /// </summary>
    ICollection<ICollectedItemItrEntity> CollectedList { get; }
}
