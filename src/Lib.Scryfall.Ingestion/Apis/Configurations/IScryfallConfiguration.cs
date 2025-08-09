namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Defines the root configuration for Scryfall API integration.
/// </summary>
public interface IScryfallConfiguration
{
    /// <summary>
    /// Configuration key for the Scryfall configuration section.
    /// </summary>
    const string ScryfallConfigKey = "ScryfallConfig";

    /// <summary>
    /// Gets the API configuration.
    /// </summary>
    /// <returns>The API configuration.</returns>
    IScryfallApiConfig ApiConfig();

    /// <summary>
    /// Gets the rate limit configuration.
    /// </summary>
    /// <returns>The rate limit configuration.</returns>
    IScryfallRateLimitConfig RateLimitConfig();

    /// <summary>
    /// Gets the cache configuration.
    /// </summary>
    /// <returns>The cache configuration.</returns>
    IScryfallCacheConfig CacheConfig();

    /// <summary>
    /// Gets the retry configuration.
    /// </summary>
    /// <returns>The retry configuration.</returns>
    IScryfallRetryConfig RetryConfig();
}
