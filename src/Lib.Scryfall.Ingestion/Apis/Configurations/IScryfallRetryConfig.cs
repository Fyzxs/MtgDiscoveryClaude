using Lib.Scryfall.Ingestion.Apis.Configurations.Values;

namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Defines the configuration for retry policies.
/// </summary>
public interface IScryfallRetryConfig
{
    /// <summary>
    /// Configuration key for maximum retries.
    /// </summary>
    const string MaxRetriesKey = "MaxRetries";

    /// <summary>
    /// Configuration key for initial delay in milliseconds.
    /// </summary>
    const string InitialDelayMsKey = "InitialDelayMs";

    /// <summary>
    /// Configuration key for maximum delay in milliseconds.
    /// </summary>
    const string MaxDelayMsKey = "MaxDelayMs";

    /// <summary>
    /// Configuration key for backoff multiplier.
    /// </summary>
    const string BackoffMultiplierKey = "BackoffMultiplier";

    /// <summary>
    /// Gets the maximum number of retry attempts.
    /// </summary>
    /// <returns>The maximum retry count.</returns>
    ScryfallMaxRetries MaxRetries();

    /// <summary>
    /// Gets the initial retry delay.
    /// </summary>
    /// <returns>The initial delay in milliseconds.</returns>
    ScryfallRetryDelay InitialDelayMs();

    /// <summary>
    /// Gets the maximum retry delay.
    /// </summary>
    /// <returns>The maximum delay in milliseconds.</returns>
    ScryfallRetryDelay MaxDelayMs();

    /// <summary>
    /// Gets the backoff multiplier for exponential retry.
    /// </summary>
    /// <returns>The backoff multiplier.</returns>
    ScryfallBackoffMultiplier BackoffMultiplier();
}
