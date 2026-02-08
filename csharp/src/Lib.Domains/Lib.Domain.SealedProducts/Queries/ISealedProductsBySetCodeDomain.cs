using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.SealedProducts.Queries;

internal interface ISealedProductsBySetCodeDomain
    : IOperationResponseService<ISealedProductsBySetCodeItrEntity, IEnumerable<ISealedProductOufEntity>>;
