namespace Lib.Shared.DataModels.Entities.Args;

/// <summary>
/// Interface for argument entities that support optional user identification for enrichment.
/// </summary>
public interface IUserIdArgEntity
{
    /// <summary>
    /// Optional user identifier for enriching results with user-specific data.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// Indicates whether a UserId has been provided.
    /// </summary>
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;

    /// <summary>
    /// Indicates whether a UserId has been provided.
    /// </summary>
    bool DoesNotHaveUserId => HasUserId is false;
}
