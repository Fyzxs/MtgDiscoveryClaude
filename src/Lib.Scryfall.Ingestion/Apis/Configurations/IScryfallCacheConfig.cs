using Lib.Scryfall.Ingestion.Apis.Configurations.Values;

namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Defines the configuration for Scryfall data caching.
/// </summary>
public interface IScryfallCacheConfig
{
    /// <summary>
    /// Configuration key for card TTL in hours.
    /// </summary>
    const string CardTtlHoursKey = "CardTtlHours";

    /// <summary>
    /// Configuration key for set TTL in hours.
    /// </summary>
    const string SetTtlHoursKey = "SetTtlHours";

    /// <summary>
    /// Configuration key for maximum cache size.
    /// </summary>
    const string MaxCacheSizeKey = "MaxCacheSize";

    /// <summary>
    /// Gets the time-to-live for cached card data.
    /// </summary>
    /// <returns>The card TTL in hours.</returns>
    ScryfallCacheTtl CardTtlHours();

    /// <summary>
    /// Gets the time-to-live for cached set data.
    /// </summary>
    /// <returns>The set TTL in hours.</returns>
    ScryfallCacheTtl SetTtlHours();

    /// <summary>
    /// Gets the maximum size of the cache.
    /// </summary>
    /// <returns>The maximum cache size.</returns>
    ScryfallMaxCacheSize MaxCacheSize();
}
