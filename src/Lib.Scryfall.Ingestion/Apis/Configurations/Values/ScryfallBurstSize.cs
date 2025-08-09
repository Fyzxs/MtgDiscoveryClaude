using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the burst size for rate limiting.
/// </summary>
public abstract class ScryfallBurstSize : ToSystemType<int>;
