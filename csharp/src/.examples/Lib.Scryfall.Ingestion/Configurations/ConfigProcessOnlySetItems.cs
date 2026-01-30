using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigProcessOnlySetItems : ProcessOnlySetItems
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigProcessOnlySetItems(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override bool AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) return false; // Default to false if not set
        if (bool.TryParse(value, out bool parsed) is false) return false; // Default to false if invalid
        return parsed;
    }
}
