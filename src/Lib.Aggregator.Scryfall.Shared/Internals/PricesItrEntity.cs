using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Internals;

internal sealed class PricesItrEntity : IPricesItrEntity
{
    private readonly dynamic _data;

    public PricesItrEntity(dynamic data) => _data = data;

    public string Usd => _data?.usd;
    public string UsdFoil => _data?.usd_foil;
    public string UsdEtched => _data?.usd_etched;
}
