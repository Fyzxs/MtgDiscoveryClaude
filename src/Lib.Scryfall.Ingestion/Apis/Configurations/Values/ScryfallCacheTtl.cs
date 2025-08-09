using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents a cache time-to-live duration in hours.
/// </summary>
public abstract class ScryfallCacheTtl : ToSystemType<int>;
