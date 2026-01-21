using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

/// <summary>
/// Public API for user sealed products collection management.
/// Provides methods for adding sealed products to a user's collection and retrieving user sealed products.
/// </summary>
public interface IUserSealedProductsEntryService
{
    Task<IOperationResponse<AddUserSealedProductResultOutEntity>> AddUserSealedProductAsync(IAddUserSealedProductArgsEntity args);
    Task<IOperationResponse<List<UserSealedProductOutEntity>>> GetUserSealedProductsByUserIdAsync(string userId);
}
