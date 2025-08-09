using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallRequestsPerSecond.
/// </summary>
internal sealed class ConfigScryfallRequestsPerSecond : ScryfallRequestsPerSecond
{
    private const int MinRequestsPerSecond = 1;
    private const int MaxRequestsPerSecond = 100;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallRequestsPerSecond(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinRequestsPerSecond || MaxRequestsPerSecond < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinRequestsPerSecond} and {MaxRequestsPerSecond} [{_sourceKey}={value}]");
        return parsed;
    }
}
