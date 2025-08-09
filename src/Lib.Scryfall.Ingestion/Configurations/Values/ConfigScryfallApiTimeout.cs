using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallApiTimeout.
/// </summary>
internal sealed class ConfigScryfallApiTimeout : ScryfallApiTimeout
{
    private const int MinTimeoutSeconds = 1;
    private const int MaxTimeoutSeconds = 300;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallApiTimeout(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinTimeoutSeconds || MaxTimeoutSeconds < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinTimeoutSeconds} and {MaxTimeoutSeconds} seconds [{_sourceKey}={value}]");
        return parsed;
    }
}
