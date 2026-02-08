using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;

internal interface IUserSealedProductsByUserIdEntryService
    : IOperationResponseService<string, IEnumerable<IUserSealedProductOufEntity>>;
