using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Apis.Configurations.Values;

/// <summary>
/// Represents the throttle warning threshold as a percentage.
/// </summary>
public abstract class ScryfallThrottleWarningThreshold : ToSystemType<double>;
