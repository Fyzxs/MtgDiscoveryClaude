using Lib.Scryfall.Ingestion.Apis.Configurations.Values;

namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Defines the configuration for Scryfall API endpoints and connection settings.
/// </summary>
public interface IScryfallApiConfig
{
    /// <summary>
    /// Configuration key for the base URL.
    /// </summary>
    const string BaseUrlKey = "BaseUrl";

    /// <summary>
    /// Configuration key for the timeout in seconds.
    /// </summary>
    const string TimeoutSecondsKey = "TimeoutSeconds";

    /// <summary>
    /// Gets the base URL for the Scryfall API.
    /// </summary>
    /// <returns>The base URL.</returns>
    ScryfallApiBaseUrl BaseUrl();

    /// <summary>
    /// Gets the timeout for API requests.
    /// </summary>
    /// <returns>The timeout in seconds.</returns>
    ScryfallApiTimeout TimeoutSeconds();
}
