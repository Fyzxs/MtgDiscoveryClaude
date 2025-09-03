using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Models;

public sealed class PricesItrEntity : IPricesItrEntity
{
    private readonly dynamic _data;

    public PricesItrEntity(dynamic data)
    {
        _data = data;
    }

    public string Usd => _data?.usd;
    public string UsdFoil => _data?.usd_foil;
    public string UsdEtched => _data?.usd_etched;
    public string Eur => _data?.eur;
    public string EurFoil => _data?.eur_foil;
    public string Tix => _data?.tix;
}