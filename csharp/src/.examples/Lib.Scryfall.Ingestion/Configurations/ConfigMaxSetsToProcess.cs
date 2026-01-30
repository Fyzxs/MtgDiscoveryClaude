using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigMaxSetsToProcess : MaxSetsToProcess
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigMaxSetsToProcess(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override int AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        if (int.TryParse(value, out int parsed) is false) throw new CosmosConfigurationException($"{GetType().Name} Invalid value [{_sourceKey}={value}]");
        return parsed;
    }

    public override bool IsUnlimited() => AsSystemType() <= 0;
}
