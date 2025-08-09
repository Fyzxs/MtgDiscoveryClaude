using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations.Values;

/// <summary>
/// Configuration-based implementation of ScryfallApiBaseUrl.
/// </summary>
internal sealed class ConfigScryfallApiBaseUrl : ScryfallApiBaseUrl
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigScryfallApiBaseUrl(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override string AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new ScryfallConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        return value;
    }
}
