using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents a retry delay duration in milliseconds.
/// </summary>
public abstract class ScryfallRetryDelay : ToSystemType<int>;
