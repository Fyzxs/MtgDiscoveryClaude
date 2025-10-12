namespace Lib.Adapter.UserSetCards.Apis.Entities;

/// <summary>
/// Transfer entity for adding or removing a card from a user's set collection.
/// Represents a single card modification operation (add or remove).
/// </summary>
public interface IAddCardToSetXfrEntity
{
    /// <summary>
    /// User identifier (partition key for Cosmos).
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// Set identifier (document ID in Cosmos).
    /// </summary>
    string SetId { get; }

    /// <summary>
    /// Card identifier being added or removed.
    /// </summary>
    string CardId { get; }

    /// <summary>
    /// Set group identifier (e.g., collector number or other grouping mechanism).
    /// </summary>
    string SetGroupId { get; }

    /// <summary>
    /// Finish type: "nonfoil", "foil", or "etched".
    /// </summary>
    string FinishType { get; }

    /// <summary>
    /// Count to add (positive) or remove (negative).
    /// </summary>
    int Count { get; }
}
