using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Args.UserCards;

/// <summary>
/// Argument entity for querying a specific user card.
/// </summary>
public interface IUserCardArgEntity : IArgEntity
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
