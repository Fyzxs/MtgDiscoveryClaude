using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the backoff multiplier for exponential retry.
/// </summary>
public abstract class ScryfallBackoffMultiplier : ToSystemType<double>;
