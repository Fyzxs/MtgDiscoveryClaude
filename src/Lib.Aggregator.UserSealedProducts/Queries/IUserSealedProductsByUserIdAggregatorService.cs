using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdAggregatorService
    : IOperationResponseService<IUserIdItrEntity, IEnumerable<IUserSealedProductItrEntity>>;
