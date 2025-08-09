using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations.Values;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

/// <summary>
/// Provides rate limit configuration for Scryfall using the configuration system.
/// </summary>
internal sealed class ConfigScryfallRateLimitConfig : IScryfallRateLimitConfig
{
    /// <summary>
    /// The configuration provider used to retrieve settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// The base configuration key for rate limit settings.
    /// </summary>
    private readonly string _baseKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallRateLimitConfig"/> class.
    /// </summary>
    /// <param name="config">The configuration provider.</param>
    public ConfigScryfallRateLimitConfig(IConfig config)
    {
        _config = config;
        _baseKey = $"{IScryfallConfiguration.ScryfallConfigKey}:RateLimit";
    }

    /// <summary>
    /// Gets the maximum number of requests allowed per second.
    /// </summary>
    /// <returns>The requests per second limit.</returns>
    public ScryfallRequestsPerSecond RequestsPerSecond()
    {
        string key = $"{_baseKey}:{IScryfallRateLimitConfig.RequestsPerSecondKey}";
        return new ConfigScryfallRequestsPerSecond(key, _config);
    }

    /// <summary>
    /// Gets the burst size for rate limiting.
    /// </summary>
    /// <returns>The burst size.</returns>
    public ScryfallBurstSize BurstSize()
    {
        string key = $"{_baseKey}:{IScryfallRateLimitConfig.BurstSizeKey}";
        return new ConfigScryfallBurstSize(key, _config);
    }

    /// <summary>
    /// Gets the threshold for throttle warnings.
    /// </summary>
    /// <returns>The throttle warning threshold.</returns>
    public ScryfallThrottleWarningThreshold ThrottleWarningThreshold()
    {
        string key = $"{_baseKey}:{IScryfallRateLimitConfig.ThrottleWarningThresholdKey}";
        return new ConfigScryfallThrottleWarningThreshold(key, _config);
    }
}
