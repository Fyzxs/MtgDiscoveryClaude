using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

namespace Cli.MtgDiscovery.PriceUpdate.Updaters;

internal interface IPriceUpdater
{
    string ContainerName { get; }
    Task<PriceUpdateItemResult> UpdatePriceAsync(string scryfallId, ManaPoolPriceItem priceItem);
}
