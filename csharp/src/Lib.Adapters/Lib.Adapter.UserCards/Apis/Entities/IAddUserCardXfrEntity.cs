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
    /// The name of the card (denormalized for display without additional lookups).
    /// </summary>
    string CardName { get; }

    /// <summary>
    /// The name of the set (denormalized for display without additional lookups).
    /// </summary>
    string SetName { get; }

    /// <summary>
    /// The code of the set (denormalized for URLs and display).
    /// </summary>
    string SetCode { get; }

    /// <summary>
    /// The release date of the card in ISO format (denormalized for sorting).
    /// </summary>
    string ReleasedAt { get; }

    /// <summary>
    /// The artist name (denormalized for display without additional lookups).
    /// </summary>
    string Artist { get; }

    /// <summary>
    /// The artist IDs for this card (used for efficient querying by artist).
    /// </summary>
    IEnumerable<string> ArtistIds { get; }

    /// <summary>
    /// The deterministic GUID generated from the card name (used for efficient querying by name).
    /// This matches the NameGuid used in the CardsByName collection.
    /// </summary>
    string CardNameGuid { get; }

    /// <summary>
    /// The details of this specific collected card version with finish, quantity, and set grouping.
    /// </summary>
    IUserCardDetailsXfrEntity Details { get; }

    /// <summary>
    /// When true, replaces existing card counts instead of adding to them.
    /// Used by migration tools to overwrite existing collection data.
    /// </summary>
    bool ReplaceMode { get; }
}
