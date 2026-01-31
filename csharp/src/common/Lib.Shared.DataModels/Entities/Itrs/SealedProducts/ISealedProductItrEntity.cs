namespace Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

public interface ISealedProductItrEntity
{
    string Uuid { get; }
    string SetId { get; }
    string SetCode { get; }
    string SetName { get; }
    string Name { get; }
    string Category { get; }
    string Subtype { get; }
    int? CardCount { get; }
    string ReleaseDate { get; }
    string TcgplayerProductId { get; }
    string ImageUrl { get; }
    string PurchaseUrlTcgplayer { get; }
    string PurchaseUrlCardmarket { get; }
    string PurchaseUrlCardKingdom { get; }
}
