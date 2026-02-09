using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdDomain
    : IOperationResponseService<IUserIdItrEntity, IEnumerable<IUserSealedProductOufEntity>>;
