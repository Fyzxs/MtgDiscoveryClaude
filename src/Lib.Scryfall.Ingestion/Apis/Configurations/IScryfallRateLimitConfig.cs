using Lib.Scryfall.Ingestion.Apis.Configurations.Values;

namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Defines the configuration for Scryfall API rate limiting.
/// </summary>
public interface IScryfallRateLimitConfig
{
    /// <summary>
    /// Configuration key for requests per second.
    /// </summary>
    const string RequestsPerSecondKey = "RequestsPerSecond";

    /// <summary>
    /// Configuration key for burst size.
    /// </summary>
    const string BurstSizeKey = "BurstSize";

    /// <summary>
    /// Configuration key for throttle warning threshold.
    /// </summary>
    const string ThrottleWarningThresholdKey = "ThrottleWarningThreshold";

    /// <summary>
    /// Gets the maximum number of requests allowed per second.
    /// </summary>
    /// <returns>The requests per second limit.</returns>
    ScryfallRequestsPerSecond RequestsPerSecond();

    /// <summary>
    /// Gets the burst size for rate limiting.
    /// </summary>
    /// <returns>The burst size.</returns>
    ScryfallBurstSize BurstSize();

    /// <summary>
    /// Gets the threshold for throttle warnings.
    /// </summary>
    /// <returns>The throttle warning threshold.</returns>
    ScryfallThrottleWarningThreshold ThrottleWarningThreshold();
}
