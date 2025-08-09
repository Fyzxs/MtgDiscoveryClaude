using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the maximum number of retry attempts.
/// </summary>
public abstract class ScryfallMaxRetries : ToSystemType<int>;
