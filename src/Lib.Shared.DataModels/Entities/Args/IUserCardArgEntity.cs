namespace Lib.Shared.DataModels.Entities.Args;

/// <summary>
/// Argument entity for querying a specific user card.
/// </summary>
public interface IUserCardArgEntity
{
    /// <summary>
    /// The ID of the user whose card to query.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The ID of the card to query.
    /// </summary>
    string CardId { get; }
}
