using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallCacheTtl.
/// </summary>
internal sealed class ConfigScryfallCacheTtl : ScryfallCacheTtl
{
    private const int MinCacheTtlHours = 1;
    private const int MaxCacheTtlHours = 10080; // 1 week

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallCacheTtl(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new ScryfallConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        if (parsed < MinCacheTtlHours || MaxCacheTtlHours < parsed) throw new ScryfallConfigurationException($"{GetType().Name} value must be between {MinCacheTtlHours} and {MaxCacheTtlHours} hours [{_sourceKey}={value}]");
        return parsed;
    }
}
