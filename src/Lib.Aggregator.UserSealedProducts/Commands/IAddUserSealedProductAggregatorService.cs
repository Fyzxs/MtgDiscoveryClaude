using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal interface IAddUserSealedProductAggregatorService
    : IOperationResponseService<IAddUserSealedProductItrEntity, IUserSealedProductOufEntity>;
