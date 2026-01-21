namespace Lib.Shared.DataModels.Entities.Args;

/// <summary>
/// Argument entity for querying user set card collection summary.
/// </summary>
public interface IUserSetCardArgEntity
{
    /// <summary>
    /// The ID of the user whose set cards to query.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The ID of the set to query.
    /// </summary>
    string SetId { get; }
}
