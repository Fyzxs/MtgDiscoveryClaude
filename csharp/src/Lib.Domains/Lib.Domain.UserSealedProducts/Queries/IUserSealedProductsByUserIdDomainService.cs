using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserSealedProducts.Queries;

/// <summary>
/// Marker interface for retrieving all sealed products for a specific user.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserSealedProductsByUserIdDomainService
    : IOperationResponseService<IUserIdItrEntity, IEnumerable<IUserSealedProductItrEntity>>;
