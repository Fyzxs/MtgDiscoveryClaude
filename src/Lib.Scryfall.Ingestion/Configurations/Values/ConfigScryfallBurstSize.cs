using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallBurstSize.
/// </summary>
internal sealed class ConfigScryfallBurstSize : ScryfallBurstSize
{
    private const int MinBurstSize = 1;
    private const int MaxBurstSize = 100;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallBurstSize(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinBurstSize || MaxBurstSize < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinBurstSize} and {MaxBurstSize} [{_sourceKey}={value}]");
        return parsed;
    }
}
