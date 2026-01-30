// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Itrs.UserCards;

/// <summary>
/// Represents a user's card collection entry following MicroObjects principles.
/// Contains all information needed to manage a user's ownership of a specific card.
/// </summary>
public interface IUserCardItrEntity : IItrEntity
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
    IUserCardDetailsItrEntity Details { get; }

    /// <summary>
    /// When true, replaces existing card counts instead of adding to them.
    /// Used by migration tools to overwrite existing collection data.
    /// </summary>
    bool ReplaceMode { get; }
}
