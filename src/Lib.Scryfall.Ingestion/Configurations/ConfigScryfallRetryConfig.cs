using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations.Values;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

/// <summary>
/// Provides retry configuration for Scryfall using the configuration system.
/// </summary>
internal sealed class ConfigScryfallRetryConfig : IScryfallRetryConfig
{
    /// <summary>
    /// The configuration provider used to retrieve settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// The base configuration key for retry settings.
    /// </summary>
    private readonly string _baseKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallRetryConfig"/> class.
    /// </summary>
    /// <param name="config">The configuration provider.</param>
    public ConfigScryfallRetryConfig(IConfig config)
    {
        _config = config;
        _baseKey = $"{IScryfallConfiguration.ScryfallConfigKey}:Retry";
    }

    /// <summary>
    /// Gets the maximum number of retry attempts.
    /// </summary>
    /// <returns>The maximum retry count.</returns>
    public ScryfallMaxRetries MaxRetries()
    {
        string key = $"{_baseKey}:{IScryfallRetryConfig.MaxRetriesKey}";
        return new ConfigScryfallMaxRetries(key, _config);
    }

    /// <summary>
    /// Gets the initial retry delay.
    /// </summary>
    /// <returns>The initial delay in milliseconds.</returns>
    public ScryfallRetryDelay InitialDelayMs()
    {
        string key = $"{_baseKey}:{IScryfallRetryConfig.InitialDelayMsKey}";
        return new ConfigScryfallRetryDelay(key, _config);
    }

    /// <summary>
    /// Gets the maximum retry delay.
    /// </summary>
    /// <returns>The maximum delay in milliseconds.</returns>
    public ScryfallRetryDelay MaxDelayMs()
    {
        string key = $"{_baseKey}:{IScryfallRetryConfig.MaxDelayMsKey}";
        return new ConfigScryfallRetryDelay(key, _config);
    }

    /// <summary>
    /// Gets the backoff multiplier for exponential retry.
    /// </summary>
    /// <returns>The backoff multiplier.</returns>
    public ScryfallBackoffMultiplier BackoffMultiplier()
    {
        string key = $"{_baseKey}:{IScryfallRetryConfig.BackoffMultiplierKey}";
        return new ConfigScryfallBackoffMultiplier(key, _config);
    }
}
