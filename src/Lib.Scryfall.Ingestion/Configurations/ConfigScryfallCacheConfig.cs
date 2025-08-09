using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations.Values;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

/// <summary>
/// Provides cache configuration for Scryfall using the configuration system.
/// </summary>
internal sealed class ConfigScryfallCacheConfig : IScryfallCacheConfig
{
    /// <summary>
    /// The configuration provider used to retrieve settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// The base configuration key for cache settings.
    /// </summary>
    private readonly string _baseKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallCacheConfig"/> class.
    /// </summary>
    /// <param name="config">The configuration provider.</param>
    public ConfigScryfallCacheConfig(IConfig config)
    {
        _config = config;
        _baseKey = $"{IScryfallConfiguration.ScryfallConfigKey}:Cache";
    }

    /// <summary>
    /// Gets the time-to-live for cached card data.
    /// </summary>
    /// <returns>The card TTL in hours.</returns>
    public ScryfallCacheTtl CardTtlHours()
    {
        string key = $"{_baseKey}:{IScryfallCacheConfig.CardTtlHoursKey}";
        return new ConfigScryfallCacheTtl(key, _config);
    }

    /// <summary>
    /// Gets the time-to-live for cached set data.
    /// </summary>
    /// <returns>The set TTL in hours.</returns>
    public ScryfallCacheTtl SetTtlHours()
    {
        string key = $"{_baseKey}:{IScryfallCacheConfig.SetTtlHoursKey}";
        return new ConfigScryfallCacheTtl(key, _config);
    }

    /// <summary>
    /// Gets the maximum size of the cache.
    /// </summary>
    /// <returns>The maximum cache size.</returns>
    public ScryfallMaxCacheSize MaxCacheSize()
    {
        string key = $"{_baseKey}:{IScryfallCacheConfig.MaxCacheSizeKey}";
        return new ConfigScryfallMaxCacheSize(key, _config);
    }
}
