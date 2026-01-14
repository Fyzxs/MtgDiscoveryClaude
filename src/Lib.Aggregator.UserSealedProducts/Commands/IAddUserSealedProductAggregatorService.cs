using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal interface IAddUserSealedProductAggregatorService
    : IOperationResponseService<IAddUserSealedProductItrEntity, List<ISealedProductOufEntity>>;
