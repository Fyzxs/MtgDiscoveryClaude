#nullable enable
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

namespace Cli.MtgDiscovery.PriceUpdate.Mapping;

internal sealed class ManaPoolToPricesMapper : IManaPoolToPricesMapper
{
    public dynamic MapToPrices(ManaPoolPriceItem item)
    {
        return new
        {
            usd = CentsToDollars(item.PriceCents),
            usd_foil = CentsToDollars(item.PriceCentsFoil),
            usd_etched = CentsToDollars(item.PriceCentsEtched)
        };
    }

    private static string? CentsToDollars(int? cents)
    {
        if (cents.HasValue is false)
        {
            return null;
        }

        return (cents.Value / 100m).ToString("F2");
    }
}
