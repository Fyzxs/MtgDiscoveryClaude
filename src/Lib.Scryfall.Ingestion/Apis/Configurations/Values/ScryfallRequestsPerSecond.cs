using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the maximum requests per second.
/// </summary>
public abstract class ScryfallRequestsPerSecond : ToSystemType<int>;
