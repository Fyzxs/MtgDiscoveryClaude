using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Cosmos.Resolvers;

namespace Lib.Adapter.UserSealedProducts.Commands.Resolvers;

internal interface IUserSealedProductResolver
    : ICosmosResolver<UserSealedProductExtEntity, IUserSealedProductXfrEntity>;
