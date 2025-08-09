using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosSasConfig : ICosmosSasConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigCosmosSasConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public ICosmosSasConnectionConfig ConnectionConfig()
    {
        string connectionKey = $"{_parentKey}:{ICosmosSasConfig.ConnectionKey}";
        return new ConfigCosmosSasConnectionConfig(connectionKey, _config);
    }
}
