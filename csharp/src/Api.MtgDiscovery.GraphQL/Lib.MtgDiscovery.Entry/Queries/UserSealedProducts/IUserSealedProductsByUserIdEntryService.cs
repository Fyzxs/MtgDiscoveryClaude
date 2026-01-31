using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;

/// <summary>
/// Entry service interface for retrieving all sealed products for a specific user.
/// Handles request validation and coordination with domain services.
/// </summary>
internal interface IUserSealedProductsByUserIdEntryService
    : IOperationResponseService<string, IEnumerable<IUserSealedProductItrEntity>>;
