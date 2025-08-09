using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallThrottleWarningThreshold.
/// </summary>
internal sealed class ConfigScryfallThrottleWarningThreshold : ScryfallThrottleWarningThreshold
{
    private const double MinThreshold = 0.0;
    private const double MaxThreshold = 1.0;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallThrottleWarningThreshold(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override double AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (double.TryParse(value, out double parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed <= MinThreshold || MaxThreshold < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinThreshold} and {MaxThreshold} [{_sourceKey}={value}]");
        return parsed;
    }
}
