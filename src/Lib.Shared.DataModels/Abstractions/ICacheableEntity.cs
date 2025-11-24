namespace Lib.Shared.DataModels.Abstractions;

/// <summary>
/// Marker interface for entities that can be cached.
/// Entities must provide their own cache key for consistent caching behavior.
/// </summary>
public interface ICacheableEntity
{
    /// <summary>
    /// Unique cache key for this entity instance.
    /// Format determined by entity type (e.g., "card:{id}", "user:{userId}:card:{cardId}").
    /// </summary>
    string CacheKey { get; }
}
