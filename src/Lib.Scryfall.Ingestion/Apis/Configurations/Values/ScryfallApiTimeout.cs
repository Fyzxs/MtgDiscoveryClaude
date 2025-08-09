using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the API timeout in seconds.
/// </summary>
public abstract class ScryfallApiTimeout : ToSystemType<int>;
