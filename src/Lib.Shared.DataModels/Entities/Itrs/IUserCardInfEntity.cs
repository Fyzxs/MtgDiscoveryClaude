// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs;

/// <summary>
/// Represents a user's card collection entry following MicroObjects principles.
/// Contains all information needed to manage a user's ownership of a specific card.
/// </summary>
public interface IUserCardItrEntity
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
    IUserCardDetailsItrEntity Details { get; }
}
