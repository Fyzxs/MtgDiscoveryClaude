using System.Collections.Concurrent;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Adapters;

internal sealed class AuthModeGenesisDevice : IGenesisDevice
{
    private static readonly ConcurrentDictionary<string, IGenesisDevice> s_cache = [];

    private readonly ILogger _logger;
    private readonly ICosmosContainerDefinition _containerDefinition;
    private readonly ICosmosConnectionConvenience _connectionConvenience;

    public AuthModeGenesisDevice(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
    {
        _logger = logger;
        _containerDefinition = containerDefinition;
        _connectionConvenience = connectionConvenience;
    }

    public async Task LiveLongAndProsper(ICosmosGenesisClientAdapter genesisClientAdapter) => await Cached().LiveLongAndProsper(genesisClientAdapter).ConfigureAwait(false);

    private IGenesisDevice Cached() => s_cache.GetOrAdd(_containerDefinition.CacheKey(), _ => NewGenesisDevice());

    private IGenesisDevice NewGenesisDevice()
    {
        ICosmosAccountConfig accountConfig = _connectionConvenience.AccountConfig(_containerDefinition);
        bool equals = CosmosAuthMode.EntraAuth.Equals(accountConfig.AuthMode().AsSystemType());
        return equals
            ? new CosmosEntraAuthGenesisDevice(_logger, _containerDefinition, _connectionConvenience)
            : new CosmosSasAuthGenesisDevice(_containerDefinition, _connectionConvenience);
    }
}
