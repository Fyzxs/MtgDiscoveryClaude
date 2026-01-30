using System.Collections.Concurrent;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Adapters;

internal sealed class MonoStateCosmosClientAdapter : ICosmosClientAdapter
{
    private readonly ICosmosContainerDefinition _containerDefinition;
    private readonly ICosmosConnectionConvenience _connectionConvenience;
    private readonly IGenesisDevice _genesisDevice;
    private static readonly ConcurrentDictionary<string, ICosmosGenesisClientAdapter> s_cache = [];
    private static readonly ConcurrentDictionary<string, bool> s_genesisComplete = [];

    public MonoStateCosmosClientAdapter(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
        : this(containerDefinition, connectionConvenience, new AuthModeGenesisDevice(logger, containerDefinition, connectionConvenience))
    { }

    private MonoStateCosmosClientAdapter(ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience, IGenesisDevice genesisDevice)
    {
        _containerDefinition = containerDefinition;
        _connectionConvenience = connectionConvenience;
        _genesisDevice = genesisDevice;
    }

    private ICosmosGenesisClientAdapter MonoState() => s_cache.GetOrAdd(_containerDefinition.CacheKey(), _ => NewAdapter());

    private ICosmosGenesisClientAdapter NewAdapter()
    {
        ICosmosAccountConfig accountConfig = _connectionConvenience.AccountConfig(_containerDefinition);
        ICosmosGenesisClientAdapter adapter = CosmosAuthMode.KeyAuth.Equals(accountConfig.AuthMode().AsSystemType())
            ? new SasAuthCosmosGenesisClientAdapter(accountConfig.SasConfig().ConnectionConfig())
            : new EntraAuthCosmosGenesisClientAdapter(_containerDefinition, _connectionConvenience);

        return adapter;
    }

    public async Task<Container> GetContainer()
    {
        ICosmosGenesisClientAdapter adapter = MonoState();
        await EnsureGenesis(adapter).ConfigureAwait(false);
        return await adapter.GetContainer(_containerDefinition).ConfigureAwait(false);
    }

    private async Task EnsureGenesis(ICosmosGenesisClientAdapter adapter)
    {
        string cacheKey = _containerDefinition.CacheKey();
        if (s_genesisComplete.ContainsKey(cacheKey)) { return; }

        await _genesisDevice.LiveLongAndProsper(adapter).ConfigureAwait(false);
        s_genesisComplete.TryAdd(cacheKey, true);
    }
}
