using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallMaxCacheSize.
/// </summary>
internal sealed class ConfigScryfallMaxCacheSize : ScryfallMaxCacheSize
{
    private const int MinCacheSizeMb = 1;
    private const int MaxCacheSizeMb = 10000; // 10GB

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallMaxCacheSize(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinCacheSizeMb || MaxCacheSizeMb < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinCacheSizeMb} and {MaxCacheSizeMb} MB [{_sourceKey}={value}]");
        return parsed;
    }
}
