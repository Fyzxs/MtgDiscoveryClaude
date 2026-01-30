using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.SealedProducts.Apis.Queries;

internal interface ISealedProductsBySetCodeAggregator
    : IOperationResponseService<ISealedProductsBySetCodeItrEntity, IEnumerable<ISealedProductOufEntity>>;
