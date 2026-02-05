using System.Collections.Generic;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.SealedProducts.Apis.Queries;

/// <summary>
/// Single-operation adapter interface for retrieving sealed products by set code.
/// </summary>
internal interface ISealedProductsBySetCodeAdapter
    : IOperationResponseService<ISealedProductsBySetCodeXfrEntity, IEnumerable<ISealedProductOufEntity>>;
