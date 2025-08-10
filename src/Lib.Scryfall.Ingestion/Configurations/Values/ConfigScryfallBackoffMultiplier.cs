using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallBackoffMultiplier.
/// </summary>
internal sealed class ConfigScryfallBackoffMultiplier : ScryfallBackoffMultiplier
{
    private const double MinMultiplier = 1.0;
    private const double MaxMultiplier = 10.0;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallBackoffMultiplier(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override double AsSystemType()
    {
        string value = _config[_sourceKey];
        if (double.TryParse(value, out double parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinMultiplier || MaxMultiplier < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinMultiplier} and {MaxMultiplier} [{_sourceKey}={value}]");
        return parsed;
    }
}
