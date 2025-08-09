using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallRetryDelay.
/// </summary>
internal sealed class ConfigScryfallRetryDelay : ScryfallRetryDelay
{
    private const int MinDelayMs = 1;
    private const int MaxDelayMs = 60_000;

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallRetryDelay(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinDelayMs || MaxDelayMs < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinDelayMs} and {MaxDelayMs} ms [{_sourceKey}={value}]");
        return parsed;
    }
}
