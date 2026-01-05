using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

/// <summary>
/// Represents a card in the signing results with signing status information.
/// </summary>
public interface ISigningCardOufEntity
{
    /// <summary>
    /// The unique identifier of the card.
    /// </summary>
    string CardId { get; }

    /// <summary>
    /// The name of the card.
    /// </summary>
    string CardName { get; }

    /// <summary>
    /// The URI of the card's image.
    /// </summary>
    string ImageUri { get; }

    /// <summary>
    /// The total number of unsigned copies of this card.
    /// </summary>
    int UnsignedCopies { get; }

    /// <summary>
    /// Indicates whether this card has both signed and unsigned copies.
    /// </summary>
    bool IsPartiallySigned { get; }

    /// <summary>
    /// Indicates whether all copies of this card are signed.
    /// </summary>
    bool IsFullySigned { get; }

    /// <summary>
    /// The detailed collection information for this card.
    /// </summary>
    IEnumerable<IUserCardDetailsOufEntity> CollectedDetails { get; }
}
