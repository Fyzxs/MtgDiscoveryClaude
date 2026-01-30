namespace Lib.Shared.Abstractions.Cache;

/// <summary>
/// Base RequestEntity to enable caching
/// </summary>
public interface ICacheableEntity
{
    /// <summary>
    /// Internal Use Only.
    /// </summary>
    string InternalCacheKey { get; }
}
