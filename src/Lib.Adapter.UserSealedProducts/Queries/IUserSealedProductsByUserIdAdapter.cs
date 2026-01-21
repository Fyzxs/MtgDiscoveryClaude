using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdAdapter
    : IOperationResponseService<string, IEnumerable<UserSealedProductExtEntity>>;
