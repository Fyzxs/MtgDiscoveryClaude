using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallMaxRetries.
/// </summary>
internal sealed class ConfigScryfallMaxRetries : ScryfallMaxRetries
{
    private const int MinRetries = 0;
    private const int MaxRetries = 10;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallMaxRetries(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinRetries || MaxRetries < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinRetries} and {MaxRetries} [{_sourceKey}={value}]");
        return parsed;
    }
}
