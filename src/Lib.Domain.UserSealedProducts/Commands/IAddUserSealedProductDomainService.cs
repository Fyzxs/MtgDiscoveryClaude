using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserSealedProducts.Commands;

internal interface IAddUserSealedProductDomainService
    : IOperationResponseService<IAddUserSealedProductItrEntity, List<ISealedProductOufEntity>>;
