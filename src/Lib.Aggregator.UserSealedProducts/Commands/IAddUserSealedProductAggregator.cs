using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal interface IAddUserSealedProductAggregator
    : IOperationResponseService<IAddUserSealedProductArgEntity, IUserSealedProductOufEntity>;
