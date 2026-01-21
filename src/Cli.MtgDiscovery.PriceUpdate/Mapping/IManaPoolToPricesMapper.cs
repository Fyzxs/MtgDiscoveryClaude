using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

namespace Cli.MtgDiscovery.PriceUpdate.Mapping;

internal interface IManaPoolToPricesMapper
{
    dynamic MapToPrices(ManaPoolPriceItem item);
}
