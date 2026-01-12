using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

/// <summary>
/// Represents a user's wishlist card entry following MicroObjects principles.
/// Contains all information needed to manage a user's wishlist for a specific card.
/// </summary>
public interface IUserWishlistCardItrEntity : IItrEntity
{
    /// <summary>
    /// The unique identifier of the user who owns this wishlist entry.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier of the card in the wishlist.
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
    /// The artist IDs for this card (used for efficient querying by artist).
    /// </summary>
    IEnumerable<string> ArtistIds { get; }

    /// <summary>
    /// The deterministic GUID generated from the card name (used for efficient querying by name).
    /// This matches the NameGuid used in the CardsByName collection.
    /// </summary>
    string CardNameGuid { get; }

    /// <summary>
    /// The details of this specific wishlisted card version with finish and quantity.
    /// </summary>
    IUserWishlistCardDetailsItrEntity Details { get; }
}
