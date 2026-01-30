using System.Collections.Generic;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

namespace Cli.MtgDiscovery.PriceUpdate.ManaPool;

internal interface IManaPoolApiClient
{
    Task<IReadOnlyDictionary<string, ManaPoolPriceItem>> FetchAllPricesAsync();
}
