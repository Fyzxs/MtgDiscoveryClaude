using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSealedProducts.Apis;

public interface IUserSealedProductsByUserIdAggregator
    : IOperationResponseService<IUserIdItrEntity, IEnumerable<IUserSealedProductItrEntity>>;
