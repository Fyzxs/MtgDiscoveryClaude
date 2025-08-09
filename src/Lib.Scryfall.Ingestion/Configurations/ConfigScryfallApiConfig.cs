using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations.Values;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

/// <summary>
/// Provides API configuration for Scryfall using the configuration system.
/// </summary>
internal sealed class ConfigScryfallApiConfig : IScryfallApiConfig
{
    /// <summary>
    /// The configuration provider used to retrieve settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// The base configuration key for API settings.
    /// </summary>
    private readonly string _baseKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigScryfallApiConfig"/> class.
    /// </summary>
    /// <param name="config">The configuration provider.</param>
    public ConfigScryfallApiConfig(IConfig config)
    {
        _config = config;
        _baseKey = $"{IScryfallConfiguration.ScryfallConfigKey}:Api";
    }

    /// <summary>
    /// Gets the base URL for the Scryfall API.
    /// </summary>
    /// <returns>The base URL.</returns>
    public ScryfallApiBaseUrl BaseUrl()
    {
        string key = $"{_baseKey}:{IScryfallApiConfig.BaseUrlKey}";
        return new ConfigScryfallApiBaseUrl(key, _config);
    }

    /// <summary>
    /// Gets the timeout for API requests.
    /// </summary>
    /// <returns>The timeout in seconds.</returns>
    public ScryfallApiTimeout TimeoutSeconds()
    {
        string key = $"{_baseKey}:{IScryfallApiConfig.TimeoutSecondsKey}";
        return new ConfigScryfallApiTimeout(key, _config);
    }
}
