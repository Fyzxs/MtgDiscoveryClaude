using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdAdapter
    : IOperationResponseService<UserSealedProductsByUserIdXfrArgs, IEnumerable<IUserSealedProductItrEntity>>;
