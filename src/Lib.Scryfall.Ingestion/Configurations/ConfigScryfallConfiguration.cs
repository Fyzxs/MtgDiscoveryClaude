using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

/// <summary>
/// Provides configuration for Scryfall API integration using the configuration system.
/// This implementation retrieves Scryfall settings from the application configuration.
/// </summary>
public sealed class ConfigScryfallConfiguration : IScryfallConfiguration
{
    /// <summary>
    /// The configuration provider used to retrieve Scryfall settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallConfiguration"/> class
    /// using the default mono-state configuration.
    /// </summary>
    public ConfigScryfallConfiguration() : this(new MonoStateConfig())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallConfiguration"/> class
    /// with the specified configuration provider.
    /// </summary>
    /// <param name="config">The configuration provider to use for retrieving Scryfall settings.</param>
    public ConfigScryfallConfiguration(IConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Gets the API configuration.
    /// </summary>
    /// <returns>The API configuration.</returns>
    public IScryfallApiConfig ApiConfig() => new ConfigScryfallApiConfig(_config);

    /// <summary>
    /// Gets the rate limit configuration.
    /// </summary>
    /// <returns>The rate limit configuration.</returns>
    public IScryfallRateLimitConfig RateLimitConfig() => new ConfigScryfallRateLimitConfig(_config);

    /// <summary>
    /// Gets the cache configuration.
    /// </summary>
    /// <returns>The cache configuration.</returns>
    public IScryfallCacheConfig CacheConfig() => new ConfigScryfallCacheConfig(_config);

    /// <summary>
    /// Gets the retry configuration.
    /// </summary>
    /// <returns>The retry configuration.</returns>
    public IScryfallRetryConfig RetryConfig() => new ConfigScryfallRetryConfig(_config);
}
