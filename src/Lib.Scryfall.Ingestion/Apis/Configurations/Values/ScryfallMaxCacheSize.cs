using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the maximum cache size in megabytes.
/// </summary>
public abstract class ScryfallMaxCacheSize : ToSystemType<int>;
