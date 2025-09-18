namespace Lib.Adapter.UserCards.Apis.Entities;

/// <summary>
/// Transfer representation of user cards set query parameters used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for user cards set query values in external system operations.
/// </summary>
public interface IUserCardsSetXfrEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier for the set.
    /// </summary>
    string SetId { get; }
}
