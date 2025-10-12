namespace Lib.Adapter.UserCards.Apis.Entities;

/// <summary>
/// Transfer representation of user card details used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for user card detail values in external system operations.
/// </summary>
public interface IUserCardDetailsXfrEntity
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
