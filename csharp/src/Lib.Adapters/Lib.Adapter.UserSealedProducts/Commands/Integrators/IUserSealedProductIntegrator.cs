using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis.Entities;

namespace Lib.Adapter.UserSealedProducts.Commands.Integrators;

internal interface IUserSealedProductIntegrator
{
    UserSealedProductExtEntity Integrate(
        UserSealedProductExtEntity current,
        IUserSealedProductXfrEntity change,
        SealedProductExtEntity product);
}
