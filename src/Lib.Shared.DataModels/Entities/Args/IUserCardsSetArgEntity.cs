namespace Lib.Shared.DataModels.Entities;

/// <summary>
/// Argument entity for querying user cards by set.
/// </summary>
public interface IUserCardsSetArgEntity
{
    /// <summary>
    /// The ID of the set to query.
    /// </summary>
    string SetId { get; }

    /// <summary>
    /// The ID of the user whose cards to query.
    /// </summary>
    string UserId { get; }
}
