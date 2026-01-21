namespace Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;

public sealed class SealedProductOutEntity
{
    public string Uuid { get; init; }
    public string SetId { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public string Subtype { get; init; }
    public int? CardCount { get; init; }
    public string ReleaseDate { get; init; }
    public string TcgplayerProductId { get; init; }
    public string ImageUrl { get; init; }
    public string PurchaseUrlTcgplayer { get; init; }
    public string PurchaseUrlCardmarket { get; init; }
    public string PurchaseUrlCardKingdom { get; init; }
}
