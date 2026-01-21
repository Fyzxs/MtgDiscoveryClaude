using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSealedProducts.Commands;

internal interface IAddUserSealedProductAdapter
    : IOperationResponseService<IUserSealedProductXfrEntity, UserSealedProductExtEntity>;
