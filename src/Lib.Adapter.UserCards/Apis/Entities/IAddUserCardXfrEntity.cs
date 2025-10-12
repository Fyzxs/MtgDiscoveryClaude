using System.Collections.Generic;

namespace Lib.Adapter.UserCards.Apis.Entities;

/// <summary>
/// Transfer representation of a user card used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for user card values in external system operations.
/// </summary>
public interface IAddUserCardXfrEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier for the card.
    /// </summary>
    string CardId { get; }

    /// <summary>
    /// The identifier of the set this card belongs to.
    /// </summary>
    string SetId { get; }

    /// <summary>
    /// The artist IDs for this card (used for efficient querying by artist).
    /// </summary>
    IEnumerable<string> ArtistIds { get; }

    /// <summary>
    /// The name of the card (used for efficient querying by name).
    /// </summary>
    string CardName { get; }

    /// <summary>
    /// The details of this specific collected card version with finish, quantity, and set grouping.
    /// </summary>
    IUserCardDetailsXfrEntity Details { get; }
}
