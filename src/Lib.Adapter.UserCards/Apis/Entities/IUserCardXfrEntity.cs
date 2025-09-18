namespace Lib.Adapter.UserCards.Apis.Entities;

/// <summary>
/// Transfer representation of user card point read parameters used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for user card query values in external system operations.
/// </summary>
public interface IUserCardXfrEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier for the card.
    /// </summary>
    string CardId { get; }
}
