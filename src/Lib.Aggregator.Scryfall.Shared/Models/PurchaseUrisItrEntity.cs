using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Models;

public sealed class PurchaseUrisItrEntity : IPurchaseUrisItrEntity
{
    private readonly dynamic _data;

    public PurchaseUrisItrEntity(dynamic data)
    {
        _data = data;
    }

    public string TcgPlayer => _data?.tcgplayer;
    public string CardMarket => _data?.cardmarket;
}