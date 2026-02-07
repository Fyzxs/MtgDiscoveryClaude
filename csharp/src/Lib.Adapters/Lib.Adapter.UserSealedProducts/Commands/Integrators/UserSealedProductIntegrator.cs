using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Universal.Primitives;

namespace Lib.Adapter.UserSealedProducts.Commands.Integrators;

internal sealed class UserSealedProductIntegrator : IUserSealedProductIntegrator
{
    private readonly TimeInstantString _timestamp;

    public UserSealedProductIntegrator()
        : this(new Iso8601UtcNowString())
    { }

    private UserSealedProductIntegrator(TimeInstantString timestamp) => _timestamp = timestamp;

    public UserSealedProductExtEntity Integrate(
        UserSealedProductExtEntity current,
        IUserSealedProductXfrEntity change,
        SealedProductExtEntity product)
    {
        int newCount = current.Count + change.CountDelta;

        return new UserSealedProductExtEntity
        {
            UserId = change.UserId,
            ProductUuid = change.ProductUuid,
            SetId = change.SetId,
            Count = newCount,
            ProductName = product.Name,
            SetCode = product.SetCode,
            SetName = product.SetName,
            Category = product.Category,
            Subtype = product.Subtype,
            ImageUrl = product.ImageUrl,
            ReleaseDate = product.ReleaseDate,
            TcgplayerProductId = product.TcgplayerProductId,
            PurchaseUrlTcgplayer = product.PurchaseUrlTcgplayer,
            PurchaseUrlCardmarket = product.PurchaseUrlCardmarket,
            PurchaseUrlCardKingdom = product.PurchaseUrlCardKingdom,
            CardCount = product.CardCount,
            UpdatedAt = _timestamp
        };
    }
}
