namespace Lib.Shared.DataModels.Entities.Itrs;

/// <summary>
/// Represents a specific version of a collected card with its finish and quantity.
/// </summary>
public interface IUserCardDetailsItrEntity
{
    /// <summary>
    /// The finish type of the card (e.g., "nonfoil", "foil", "etched").
    /// </summary>
    string Finish { get; }

    /// <summary>
    /// Any special characteristics of this card version (e.g., "none", "altered", "signed", "proof").
    /// </summary>
    string Special { get; }

    /// <summary>
    /// The number of cards of this specific version owned by the user.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// The set grouping ID for the card (e.g., 'borderless', 'showcase').
    /// This value is used for UserSetCards aggregation but not persisted in UserCards.
    /// </summary>
    string SetGroupId { get; }
}
